using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace Gateways
{
    public interface IEfawateerProxy
    {
        string SendSoapRequest(string request, string soapAction, int timeout, out long timeExecuted);
        string SendSoapRequest(string request, string soapAction, int timeout);
    }

    public class EfawateerProxy : IEfawateerProxy
    {
        private readonly string _uri;
        private readonly Action<string> _detailLog;

        public EfawateerProxy(string uri, Action<string> detailLog)
        {
            _uri = uri;
            _detailLog = detailLog;
        }

        public string SendSoapRequest(string request, string soapAction, int timeout, out long timeExecuted)
        {
            HttpWebResponse resp = null;
            try
            {
                const int BUFFER_SIZE = 4096;
                string threadID = Thread.CurrentThread.ManagedThreadId.ToString();

                if (_detailLog != null)
                    _detailLog(string.Format("{1}{0}:{2}", threadID, soapAction, request));

                // Странная рекоммендация от http://bytes.com/topic/c-sharp/answers/228437-httpwebresponses-getresponse-hangs-timeouts
                int tmp = ServicePointManager.DefaultConnectionLimit;

                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(_uri);
                req.Timeout = timeout;
                req.KeepAlive = false;

                req.Method = "POST";
                req.ContentType = "text/xml; charset=utf-8";
                req.Headers.Add("SOAPAction", soapAction);

                byte[] btSend = System.Text.Encoding.UTF8.GetBytes(request);
                req.ContentLength = btSend.Length;

                DateTime t1 = DateTime.Now;

                using (Stream webStream = req.GetRequestStream())
                {
                    webStream.Write(btSend, 0, btSend.Length);
                    webStream.Flush();
                    webStream.Close();
                    resp = (HttpWebResponse)req.GetResponse();

                    Stream streamResponse = resp.GetResponseStream();
                    using (StreamReader readStream = new StreamReader(streamResponse, Encoding.UTF8))
                    {
                        Char[] read = new Char[BUFFER_SIZE];
                        int count = readStream.Read(read, 0, BUFFER_SIZE);
                        StringBuilder sb = new StringBuilder();
                        while (count > 0)
                        {
                            sb.Append(read, 0, count);
                            count = readStream.Read(read, 0, BUFFER_SIZE);
                        }
                        streamResponse.Close();
                        readStream.Close();

                        DateTime t2 = DateTime.Now;
                        TimeSpan ts = t2 - t1;

                        timeExecuted = ts.Ticks;

                        if (_detailLog != null)
                            _detailLog(string.Format("rsp{0}:{1}", threadID, sb.ToString()));

                        return sb.ToString();
                    }
                }
            }
            finally
            {
                if (resp != null)
                    resp.Close();
            }   
        }

        public string SendSoapRequest(string request, string soapAction, int timeout)
        {
            long timeExecuted;

            return SendSoapRequest(request, soapAction, timeout, out timeExecuted);
        }
    }
}