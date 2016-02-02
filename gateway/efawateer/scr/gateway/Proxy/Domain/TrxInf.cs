using System.Xml.Serialization;

namespace EfawateerGateway.Proxy.Domain
{
    [XmlRoot("TrxInf")]
    public class TrxInf
    {
        [XmlElement("AcctInfo")]
        public AcctInfo AcctInfo { get; set; }
        [XmlElement("BankTrxID")]
        public string BankTrxID { get; set; }
        [XmlElement("ValidationCode")]
        public string ValidationCode { get; set; }
        [XmlElement("PmtStatus")]
        public PmtStatus PmtStatus { get; set; }
        [XmlElement("DueAmt")]
        public string DueAmt { get; set; }
        [XmlElement("PaidAmt")]
        public string PaidAmt { get; set; }
        [XmlElement("ProcessDate")]
        public string ProcessDate { get; set; }
        [XmlElement("AccessChannel")]
        public AccessChannel AccessChannel { get; set; }
        [XmlElement("PaymentMethod")]
        public PaymentMethod PaymentMethod { get; set; }
        [XmlElement("ServiceTypeDetails")]
        public ServiceTypeDetails ServiceTypeDetails { get; set; }
    }
}