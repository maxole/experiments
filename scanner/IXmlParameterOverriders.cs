using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Core.Attributes;
using Core.Extensions;
using Core.IoC;

namespace Core.Scanner
{
    public interface IXmlParameterOverriders
    {
        Dictionary<string, XmlSerializer> Overriders { get; }
    }

    public class XmlParameterOverriders : IXmlParameterOverriders
    {
        public Dictionary<string, XmlSerializer> Overriders { get; private set; }

        public XmlParameterOverriders(IObjectFactory factory)
        {
            Overriders = factory.GetTypesWithTag(ScannerElementConvention.Tag)
                .ToDictionary(t => t.Attribute<ScanningElementAttribute>().ElementName, t => new XmlSerializer(t));
        }

        public XmlParameterOverriders(Dictionary<string, XmlSerializer> dictionary)
        {
            Overriders = dictionary;
        }
    }
}
