using System.Xml.Serialization;

namespace EfawateerGateway.Proxy.Domain
{
    public class AcctInfo
    {
        [XmlElement("BillingNo")]
        public string BillingNo { get; set; }
        [XmlElement("BillNo")]
        public string BillNo { get; set; }
        [XmlElement("BillerCode")]
        public int BillerCode { get; set; }
    }

    public class AcctInfo2
    {
        //[XmlElement("BillingNo")]
        //public string BillingNo { get; set; }
        [XmlElement("BillerCode")]
        public int BillerCode { get; set; }
        [XmlElement("BillNo")]
        public string BillNo { get; set; }        
    }
}