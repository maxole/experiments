using System.Collections.Generic;
using System.Xml.Serialization;

namespace EfawateerGateway
{
    public interface IXmlParameterOverriders
    {
        Dictionary<string, XmlSerializer> Overriders { get; }
    }
    
    public class XmlParameterOverriders : IXmlParameterOverriders
    {
        public Dictionary<string, XmlSerializer> Overriders { get; private set; }

        public XmlParameterOverriders(Dictionary<string, XmlSerializer> dictionary)
        {
            Overriders = dictionary;
        }
    }
}
