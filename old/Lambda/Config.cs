using System.Configuration;
using Lambda.GenH30.Properties;

namespace Lambda.GenH30
{
    /// <summary>
    /// 
    /// </summary>
    public class Config : ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        public byte Address
        {
            get { return (byte)this["address"]; }
            set { this["address"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Idn
        {
            get { return (string)this["idn"]; }
            set { this["idn"] = value; }
        }
        /// <summary>
        /// напряжение 
        /// </summary>
        public float Voltage
        {
            get { return (float)this["voltage"]; }
            set { this["voltage"] = value; }
        }
        /// <summary>
        /// ток  
        /// </summary>
        public float Current
        {
            get { return (float)this["current"]; }
            set { this["current"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Config()
        {
            _properties = new ConfigurationPropertyCollection
            {
                _address,
                _idn,
                _voltage,
                _current
            };
        }

        private static ConfigurationPropertyCollection _properties;

        private static readonly ConfigurationProperty _address = new ConfigurationProperty("address", typeof(byte), (byte)6, ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _idn = new ConfigurationProperty("idn", typeof(string), "", ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _voltage = new ConfigurationProperty("voltage", typeof(float), 27.0f, ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _current = new ConfigurationProperty("current", typeof(float), 6.0f, ConfigurationPropertyOptions.IsRequired);

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}