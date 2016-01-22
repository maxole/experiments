using System.Xml.Serialization;

namespace EfawateerGateway.Proxy.Domain
{
    [XmlRoot("Transactions")]
    public class Transactions
    {
        public TrxInf TrxInf { get; set; }
    }
}