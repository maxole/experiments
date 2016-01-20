using System.Configuration;
using System.IO.Ports;

namespace LFGenerator2.Configuration
{
    public class TransportConfiguration : ConfigurationSection    
    {
        [ConfigurationProperty("portName", IsRequired = true)]
        public string PortName
        {
            get { return (string)this["portName"]; }
            set { this["portName"] = value; }
        }

        [ConfigurationProperty("baudRate", DefaultValue = (int)9600)]
        public int BaudRate
        {
            get { return (int)this["baudRate"]; }
            set { this["baudRate"] = value; }
        }

        [ConfigurationProperty("stopBits", DefaultValue = StopBits.One)]
        public StopBits StopBits
        {
            get { return (StopBits)this["stopBits"]; }
            set { this["stopBits"] = value; }
        }

        [ConfigurationProperty("parity", DefaultValue = Parity.None)]
        public Parity Parity
        {
            get { return (Parity)this["parity"]; }
            set { this["parity"] = value; }
        }

        [ConfigurationProperty("dataBits", DefaultValue = 8)]
        public int DataBits
        {
            get { return (int)this["dataBits"]; }
            set { this["dataBits"] = value; }
        }
    }
}
