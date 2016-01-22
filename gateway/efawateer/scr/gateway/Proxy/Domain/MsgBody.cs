using System.Xml.Serialization;

namespace EfawateerGateway.Proxy.Domain
{
    [XmlRoot("MsgBody")]
    public class MsgBody
    {
        [XmlElement]
        public Transactions Transactions { get; set; }
    }
}