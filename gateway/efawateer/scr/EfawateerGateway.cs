using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml;

namespace Gateways
{
    public class EfawateerGateway : BaseGateway, IGateway, IEfawateerProxy
    {
        private bool _detailLogEnabled;
        private string _crtPass;
        private X509Certificate2 _cert;
        private IEfawateerProxy _proxy;

        public EfawateerGateway()
        {            
        }

        public EfawateerGateway(EfawateerGateway gateway)
        {
            _detailLogEnabled = gateway._detailLogEnabled;
            _crtPass = gateway._crtPass;
            _cert = new X509Certificate2(gateway._cert);
            _proxy = this;

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

                if (xmlData.DocumentElement["crt_pass"] != null)
                    _crtPass = xmlData.DocumentElement["crt_pass"].InnerText;

                if (_crtPass != string.Empty)
                {
                    log("Loading Certificate with password");
                    _cert = new X509Certificate2(Encoding.GetEncoding(1251).GetBytes(xmlData.DocumentElement["_cert"].InnerText.Trim()), _crtPass);
                }
                else
                {
                    log("Loading Certificate without password");
                    _cert = new X509Certificate2(Encoding.GetEncoding(1251).GetBytes(xmlData.DocumentElement["_cert"].InnerText.Trim()));
                }

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
                if (_cert != null)
                {
                    var store = new X509Store(StoreName.AuthRoot, StoreLocation.LocalMachine);
                    store.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadWrite);
                    store.Add(_cert);
                    store.Close();
                }

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
    }
}
