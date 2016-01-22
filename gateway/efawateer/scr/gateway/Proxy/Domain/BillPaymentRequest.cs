namespace EfawateerGateway.Proxy.Domain
{
    public class BillPaymentRequest
    {
        public MsgHeader MsgHeader { get; set; }
        public MsgBody MsgBody { get; set; }
        public MsgFooter MsgFooter { get; set; }
    }
}