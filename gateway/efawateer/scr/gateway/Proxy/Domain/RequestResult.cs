using System;
using System.Xml.Serialization;

namespace EfawateerGateway.Proxy.Domain
{
    public class RequestResult
    {
        [XmlElement("MsgHeader")]
        public MsgHeaderImlp MsgHeader { get; set; }
        [XmlElement("MsgBody")]
        public MsgBodyImpl MsgBody { get; set; }
        [XmlElement("MsgFooter")]
        public MsgFooter MsgFooter { get; set; }

        public class MsgHeaderImlp
        {
            public MsgHeaderImlp()
            {
                Result = new ResultImpl();
            }

            [XmlElement("TmStp")]
            public string TmStp { get; set; }

            [XmlElement("GUID")]
            public Guid Guid { get; set; }

            [XmlElement("TrsInf")]
            public TrsInfImpl TrsInf { get; set; }

            [XmlElement("Result")]
            public ResultImpl Result { get; set; }

            public class TrsInfImpl
            {
                [XmlElement("RcvCode")]
                public int RcvCode { get; set; }

                [XmlElement("ResTyp")]
                public string ResTyp { get; set; }
            }

            public class ResultImpl
            {
                [XmlElement("ErrorCode")]
                public int ErrorCode { get; set; }

                [XmlElement("ErrorDesc")]
                public string ErrorDesc { get; set; }

                [XmlElement("Severity")]
                public Severity Severity { get; set; }
            }
        }

        public class MsgBodyImpl
        {
            [XmlElement("TokenConf")]
            public TokenConfImpl TokenConf { get; set; }
        }

        public class TokenConfImpl
        {
            [XmlElement("TokenKey")]
            public string TokenKey { get; set; }
            [XmlElement("ExpiryDate")]
            public string ExpiryDate { get; set; }
        }
    }
}