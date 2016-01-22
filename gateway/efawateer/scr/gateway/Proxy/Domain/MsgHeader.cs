using System.Xml.Serialization;

namespace EfawateerGateway.Proxy.Domain
{
    [XmlRoot("MsgHeader")]
    public class MsgHeader
    {
        [XmlElement("TmStp", Order = 1)]
        public string TmStp { get; set; }

        [XmlElement("TrsInf", Order = 2)]
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