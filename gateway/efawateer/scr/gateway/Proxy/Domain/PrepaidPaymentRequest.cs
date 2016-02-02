using System;
using System.Xml.Serialization;

namespace EfawateerGateway.Proxy.Domain
{
    public class PrepaidPaymentRequest
    {
        public MsgHeader2 MsgHeader { get; set; }
        public MsgBody2 MsgBody { get; set; }
        public MsgFooter MsgFooter { get; set; }
    }

    [XmlRoot("MsgBody")]
    public class MsgBody2
    {
        public TrxInf TrxInf { get; set; }
    }

    [XmlRoot("MsgHeader")]
    public class MsgHeader2
    {
        [XmlElement("TmStp", Order = 1)]
        public string TmStp { get; set; }

        [XmlElement("GUID", Order = 2)]
        public string Guid { get; set; }

        [XmlElement("TrsInf", Order = 3)]
        public TrsInfImpl TrsInf { get; set; }

        public class TrsInfImpl
        {
            [XmlElement("SdrCode", Order = 1)]
            public int SdrCode { get; set; }

            [XmlElement("RcvCode", Order = 2)]
            public int RcvCode { get; set; }

            [XmlElement("ReqTyp", Order = 3)]
            public string ReqTyp { get; set; }
        }
    }
}