using System;
using System.Data;
using System.Xml;

namespace Gateways
{
    public class EfawateerGateway : BaseGateway, IGateway
    {
        private bool _detailLogEnabled;
        private string _password;
        private string _privateKey;
        private IEfawateerProxy _proxy;
        private string _serviceUri;

        public EfawateerGateway()
        {            
        }

        public EfawateerGateway(EfawateerGateway gateway)
        {
            _detailLogEnabled = gateway._detailLogEnabled;
            _password = gateway._password;
            _privateKey = gateway._privateKey;
            _serviceUri = gateway._serviceUri;

            // base copy
            Copy(this);
        }

        public void Initialize(string data)
        {
            log("Initialize, GateProfileID=" + GateProfileID);
            try
            {
                var xmlData = new XmlDocument();
                xmlData.LoadXml(data);

                _serviceUri = xmlData.DocumentElement["url"].InnerText;

                if (xmlData.DocumentElement["crt_pass"] != null)
                    _password = xmlData.DocumentElement["crt_pass"].InnerText;

                if (xmlData.DocumentElement["crt_key"] != null)
                    _privateKey = xmlData.DocumentElement["crt_key"].InnerText.Trim();

                var singer = new EfawateerSigner(_privateKey, _password);
                singer.CheckCerificate();

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
                log("Initialize exception: " + ex);
                throw;
            }
        }

        public void ProcessPayment(object paymentData, object operatorData, object exData)
        {
            var initialSession = string.Empty;
            log("Efawateer processing...");
            try
            {
                var paymentRow = paymentData as DataRow;
                if (paymentRow == null) throw new Exception("unable to extract paymentData");

                var operatorRow = operatorData as DataRow;

                initialSession = (paymentRow["InitialSessionNumber"] as string);
                var session = (paymentRow["SessionNumber"] is DBNull) ? "" : (paymentRow["SessionNumber"] as string);

                var ap = (int)paymentRow["TerminalID"];
                var status = (int)paymentRow["StatusID"];
                var errorCode = (int)paymentRow["ErrorCode"];
                var paymentParams = paymentRow["Params"] as string;


            }
            catch (Exception ex)
            {
                log(string.Format("ProcessPayment (initial_session={0}) exception: {1}", initialSession, ex.Message));
            }
        }

        public override string ProcessOnlineCheck(NewPaymentData paymentData, object operatorData)
        {
            int responseResult = 0;
            try
            {
                var operatorRow = operatorData as DataRow;
                if (operatorRow == null) throw new Exception("unable to extract paymentData");

                var operatorFormatString = operatorRow["OsmpFormatString"] is DBNull ? "" : operatorRow["OsmpFormatString"] as string;
                var formatedPaymentParams = FormatParameters(paymentData.Params, operatorFormatString);

                string sessionEx = GenerateSessionNumber12Digits();

            }
            catch (Exception ex)
            {
                log("ProcessOnlineCheck exception: " + ex.Message);
                responseResult = 30;
            }

            string responseString = "DATE=" + DateTime.Now.ToString("ddMMyyyy HHmmss") + "\r\n" + "SESSION=" +
                        paymentData.Session + "\r\n" + "ERROR=" + responseResult + "\r\n" + "RESULT=" +
                        ((responseResult == 0) ? "0" : "1") + "\r\n";

            return responseString;
        }

        public string CheckSettings()
        {
            var message = string.Empty;
            try
            {
                var signer = new EfawateerSigner(_privateKey, _password);
                signer.CheckCerificate();

                // todo добавить проверку параметров
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

        private string SendRequest(string request, string action)
        {            
            var signer = new EfawateerSigner(_privateKey, _password);
            _proxy = new EfawateerProxy(_serviceUri, m => { if (_detailLogEnabled) DetailLog(m); });
            var response = _proxy.SendSoapRequest(request, action, timeout);
            if (!signer.VerifyData(response))
                throw new Exception("Response has bad signature");
            return response;
        }
    }
}
