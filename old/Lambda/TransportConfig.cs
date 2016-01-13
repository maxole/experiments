using System.ComponentModel;
using System.Configuration;

namespace Lambda.GenH30
{
    /// <summary>
    /// 
    /// </summary>
    public class TransportConfig : ConfigurationSection
    {
        /// <summary>
        /// 
        /// </summary>
        public string PortName
        {
            get { return (string)this["portName"]; }
            set { this["portName"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int BaudRate
        {
            get { return (int)this["baudRate"]; }
            set { this["baudRate"] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Adr
        {
            get { return (int)this["adr"]; }
            set { this["adr"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Out
        {
            get { return (int)this["out"]; }
            set { this["out"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Idn
        {
            get { return (int)this["idn"]; }
            set { this["idn"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Dvc
        {
            get { return (int)this["dvc"]; }
            set { this["dvc"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Pv
        {
            get { return (int)this["pv"]; }
            set { this["pv"] = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Pc
        {
            get { return (int)this["pc"]; }
            set { this["pc"] = value; }
        }
        /// <summary>
        /// количество попыток в случае неполучения ответа на запрос
        /// </summary>
        public int RepeatCount
        {
            get { return (int)this["repeatcount"]; }
            set { this["repeatcount"] = value; }
        }
        /// <summary>
        /// задержка повторной посылки последнего запроса запроса
        /// </summary>
        public int RepeatTimeout
        {
            get { return (int)this["repeattimeout"]; }
            set { this["repeattimeout"] = value; }
        }

        private static ConfigurationPropertyCollection _properties;

        private static readonly ConfigurationProperty _portName = new ConfigurationProperty("portName", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _baudRate = new ConfigurationProperty("baudRate", typeof(int), 19200, TypeDescriptor.GetConverter(typeof(int)), new IntegerValidator(1200, 19200), ConfigurationPropertyOptions.IsRequired, "Bound rate");
        private static readonly ConfigurationProperty _adr = new ConfigurationProperty("adr", typeof(int), 100, ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _out = new ConfigurationProperty("out", typeof(int), 100, ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _idn = new ConfigurationProperty("idn", typeof(int), 1000, ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _dvc = new ConfigurationProperty("dvc", typeof(int), 1000, ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _pv = new ConfigurationProperty("pv", typeof(int), 100, ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _pc = new ConfigurationProperty("pc", typeof(int), 100, ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _repeatcount = new ConfigurationProperty("repeatcount", typeof(int), 2, ConfigurationPropertyOptions.IsRequired);
        private static readonly ConfigurationProperty _repeattimeout = new ConfigurationProperty("repeattimeout", typeof(int), 1000, ConfigurationPropertyOptions.IsRequired);

        /// <summary>
        /// 
        /// </summary>
        public TransportConfig()
        {
            _properties = new ConfigurationPropertyCollection
            {
                _portName,
                _baudRate,
                _adr,
                _dvc,
                _idn,
                _out,
                _pv,
                _pc,
                _repeatcount,
                _repeattimeout
            };
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }
    }
}