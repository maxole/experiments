using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Xml;
using System.Xml.Linq;
using EfawateerGateway.Proxy.Service;
using Gateways.Utils;

namespace Gateways
{
    public class EfawateerGateway : BaseGateway, IGateway
    {
        private bool _detailLogEnabled;

        private string _customerCode;
        private string _password;
        private string _certificate;

        private string _tokenUrl;
        private string _inquiryUrl;
        private string _paymentUrl;

        private const string BillInquiry = "billInquiry";
        private const string BillPayment = "billPayment";

        public EfawateerGateway()
        {            
        }

        public EfawateerGateway(EfawateerGateway gateway)
        {
            _detailLogEnabled = gateway._detailLogEnabled;

            _customerCode = gateway._customerCode;
            _password = gateway._password;
            _certificate = gateway._certificate;

            _tokenUrl = gateway._tokenUrl;
            _inquiryUrl = gateway._inquiryUrl;
            _paymentUrl = gateway._paymentUrl;

            // base copy
            Copy(this);
        }

        public void Initialize(string data)
        {
#if !TEST
            log("Initialize, GateProfileID=" + GateProfileID);
#endif
            try
            {
                var xmlData = new XmlDocument();
                xmlData.LoadXml(data);

                _tokenUrl = xmlData.DocumentElement["token_url"].InnerText;
                _inquiryUrl = xmlData.DocumentElement["inquiry_url"].InnerText;
                _paymentUrl = xmlData.DocumentElement["payment_url"].InnerText;

                _customerCode = xmlData.DocumentElement["customer_code"].InnerText;
                _password = xmlData.DocumentElement["password"].InnerText;
                _certificate = xmlData.DocumentElement["crt"].InnerText;

                if (xmlData.DocumentElement["detail_log"] != null &&
                    (xmlData.DocumentElement["detail_log"].InnerText.ToLower() == "true" ||
                     xmlData.DocumentElement["detail_log"].InnerText == "1"))
                {
                    _detailLogEnabled = true;
                }
                else
                    _detailLogEnabled = false;
            }
            catch (Exception ex)
            {
#if !TEST
                log("Initialize exception: " + ex);
                throw;
#endif
            }
        }

        public void ProcessPayment(object paymentData, object operatorData, object exData)
        {
            var initial_session = string.Empty;
#if !TEST
            log("Efawateer processing...");
#endif
            try
            {
                var paymentRow = paymentData as DataRow;
                if (paymentRow == null) throw new Exception("unable to extract paymentData");

                var operatorRow = operatorData as DataRow;
                if (operatorRow == null) throw new Exception("unable to extract operatorRow");

                initial_session = (paymentRow["InitialSessionNumber"] as string);
                var session = (paymentRow["SessionNumber"] is DBNull) ? "" : (paymentRow["SessionNumber"] as string);

                var ap = (int) paymentRow["TerminalID"];
                var status = (int) paymentRow["StatusID"];
                var errorCode = (int) paymentRow["ErrorCode"];
                var paymentParams = paymentRow["Params"] as string;

                if ((session.Length == 0) && ((status == 103) || (status == 104)))
                {
#if !TEST
                    PreprocessPaymentStatus(ap, initial_session, errorCode, (status == 103) ? 102 : 100, exData);
#endif
                    return;
                }

                if (status == 109)
                {
                    log("Отмена счета не поддерживается");
                    return;
                }

                string operatorFormatString = operatorRow["OsmpFormatString"] is DBNull ? "" : operatorRow["OsmpFormatString"] as string;

                var formatedPaymentParams = FormatParameters(paymentParams, operatorFormatString);
                var parametersList = new StringList(formatedPaymentParams, ";");

                var amount = otof(paymentRow["Amount"]);
                var amountAll = otof(paymentRow["AmountAll"]);

                if (session.Length == 0)
                    session = GenerateSessionNumber12Digits();

                var key = Authenticate();

                var request = GetRequestContent(BillPayment);

                var sign = new EfawateerSigner(_certificate);

                var time = DateTime.Now.ToString("s");
                request.Element("MsgHeader").Element("TmStp").Value = time;
                request.Element("MsgHeader").Element("TrsInf").Element("SdrCode").Value = _customerCode;

                var trxInfo = request.Element("MsgBody").Element("Transactions").Element("TrxInf");

                var acctInfo = trxInfo.Element("AcctInfo");
                acctInfo.Element("BillingNo").Value = parametersList["billingno"];
                acctInfo.Element("BillNo").Value = parametersList["billingno"];
                acctInfo.Element("BillerCode").Value = parametersList["billingcode"];

                trxInfo.Element("BankTrxID").Value = session;
                trxInfo.Element("DueAmt").Value = amount.ToString(CultureInfo.InvariantCulture);
                trxInfo.Element("PaidAmt").Value = amountAll.ToString(CultureInfo.InvariantCulture);
                trxInfo.Element("ProcessDate").Value = time;

                request.Element("MsgFooter").Element("Security").Element("Signature").Value =
                    sign.SignData(request.Element("MsgBody").ToString());

                if (_detailLogEnabled)
                    log("Payment request:" + request);

                var service = new BillPaymentClient(new WSHttpBinding(SecurityMode.None, true),
                    new EndpointAddress(_paymentUrl));
                var response = service.PayBill(GenerateGuid(), key, request);

                if (_detailLogEnabled)
                    log("Payment response:" + response);

                int code = 0;

                var body = response.Element("MsgBody");
                if (body == null)
                {
                    code = Convert.ToInt32(response.Element("MsgHeader").Element("Result").Element("ErrorCode").Value);
#if !TEST
                    PreprocessPaymentStatus(ap, initial_session, EfawateerCodeToCyberCode(code), status, exData);
#endif
                    return;
                }

                trxInfo = body.Element("Transactions").Element("TrxInf");
                code = Convert.ToInt32(trxInfo.Element("Result").Element("ErrorCode").Value);
                if (code != 0)
                {
#if !TEST
                    PreprocessPaymentStatus(ap, initial_session, EfawateerCodeToCyberCode(code), status, exData);
#endif
                    return;
                }

                session = trxInfo.Element("JOEBPPSTrx").Value;

#if !TEST
                PreprocessPayment(ap, initial_session, session, DateTime.Now, exData);
                PreprocessPaymentStatus(ap, initial_session, EfawateerCodeToCyberCode(code), status, exData);
#endif

            }
            catch (Exception ex)
            {
#if !TEST
                log(string.Format("ProcessPayment (initial_session={0}) exception: {1}", initial_session, ex.Message));
#else
                throw;
#endif
            }
        }

        public override string ProcessOnlineCheck(NewPaymentData paymentData, object operatorData)
        {
            int responseResult = 0;
            string param = string.Empty;
            try
            {
                var operatorRow = operatorData as DataRow;
                if (operatorRow == null) throw new Exception("unable to extract paymentData");

                var operatorFormatString = operatorRow["OsmpFormatString"] is DBNull
                    ? ""
                    : operatorRow["OsmpFormatString"] as string;
                var formatedPaymentParams = FormatParameters(paymentData.Params, operatorFormatString);
                var parametersList = new StringList(formatedPaymentParams, ";");
                
                var token = Authenticate();

                var request = GetRequestContent(BillInquiry);
                
                var signer = new EfawateerSigner(_certificate);
                
                request.Element("MsgHeader").Element("TmStp").Value = DateTime.Now.ToString("s");
                request.Element("MsgHeader").Element("TrsInf").Element("SdrCode").Value = _customerCode;
                request.Element("MsgBody").Element("AcctInfo").Element("BillingNo").Value = parametersList["billingno"];
                request.Element("MsgBody").Element("AcctInfo").Element("BillerCode").Value = parametersList["billingcode"];
                request.Element("MsgFooter").Element("Security").Element("Signature").Value = signer.SignData(request.Element("MsgBody").ToString());

                if (_detailLogEnabled)
                    log("Inquire request:" + request);

                var service = new BillInquiryClient(new WSHttpBinding(SecurityMode.None, true), new EndpointAddress(_inquiryUrl));
                var response = service.Inquire(GenerateGuid(), token, request);

                if (_detailLogEnabled)
                    log("Inquire response:" + response);

                var body = response.Element("MsgBody");
                if (body == null)
                    throw new Exception(string.Format("unable get bill inquery: {0}", response.Element("MsgHeader").Element("Result").ToString()));


                var count = Convert.ToInt32(body.Element("RecCount").Value);
                if(count > 1)
                    throw new Exception("founded more whan 1 bills record");

                var bill = body.Element("BillsRec").Element("BillRec");
                var due = bill.Element("DueAmount").Value;
                var fee = bill.Element("FeesAmt").Value;

                param = string.Format("DUE={0}\r\nFEE={1}", due, fee);
            }
            catch (Exception ex)
            {
#if !TEST
                log("ProcessOnlineCheck exception: " + ex.Message);
                responseResult = 30;
#else
                throw;
#endif                
            }

            string responseString = "DATE=" + DateTime.Now.ToString("ddMMyyyy HHmmss") + "\r\n" + "SESSION=" +
                                    paymentData.Session + "\r\n" + "ERROR=" + responseResult + "\r\n" + "RESULT=" +
                                    ((responseResult == 0) ? "0" : "1") + "\r\n" + param + "\r\n";

            return responseString;
        }

        public string CheckSettings()        
        {
            /*
             * проверяется только успешность получения токена, остальные службы не проверяются.
             * они должны работать в комплексе, и если нет доступа к этой - то работоспособность 
             * остальных не имеет особого значения
             */
            var message = string.Empty;
            try
            {
                Authenticate();

                message = "OK";
            }
            catch (Exception ex)
            {
                message += " (Exception): " + ex.Message;
            }

            return message;
        }

        public IGateway Clone()
        {
            return new EfawateerGateway(this);
        }

        private string GenerateGuid()
        {
            return Guid.NewGuid().ToString();
        }

        private string Authenticate()
        {
            var token = new TokenServiceClient(new WSHttpBinding(SecurityMode.None, true), new EndpointAddress(_tokenUrl));
            var authenticate = token.Authenticate(GenerateGuid(), Convert.ToInt32(_customerCode), _password);
            if (_detailLogEnabled)
                log("Authenticate" + authenticate);
            var body = authenticate.Element("MsgHeader");
            if (body == null)
                throw new Exception(string.Format("unable get token: {0}", authenticate.Element("MsgHeader").Element("Result").ToString()));

            return authenticate.Element("MsgBody").Element("TokenConf").Element("TokenKey").Value;
        }

        private XElement GetRequestContent(string request)
        {            
            var assembly = Assembly.GetExecutingAssembly();
            using (var stream = assembly.GetManifestResourceStream(string.Format("EfawateerGateway.Proxy.{0}.xml", request)))
            using (var reader = new StreamReader(stream))
                return XElement.Parse(reader.ReadToEnd());
        }

        private int EfawateerCodeToCyberCode(int code)
        {
            if (code == 0)
                return 0;
            return 2100000 + code;
        }
    }
}
