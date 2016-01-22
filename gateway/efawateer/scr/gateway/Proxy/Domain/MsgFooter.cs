using System.Xml.Serialization;

namespace EfawateerGateway.Proxy.Domain
{
    [XmlRoot("MsgFooter")]
    public class MsgFooter
    {
        public SecurityImlp Security { get; set; }

        [XmlRoot("Security")]
        public class SecurityImlp
        {
            [XmlElement("Signature")]
            public string Signature { get; set; }
        }
    }
}