using System.Xml.Serialization;

namespace EfawateerGateway.Proxy.Domain
{
    public class ServiceTypeDetails
    {
        [XmlElement("ServiceType")]
        public ServiceType ServiceType { get; set; }
        [XmlElement("PrepaidCat")]
        public string PrepaidCat { get; set; }
    }
}