using System.ComponentModel;
using ConfiguratorDecorator;

namespace PluginWithDecorator
{
    /// <summary>
    /// декоратор для конкретных настроек
    /// </summary>
    public class ConfiguratorDecorator : LocalizeConfiguratorDecorator<PluginConfiguration>
    {
        public ConfiguratorDecorator() 
            : base(new PluginConfiguration())
        {         
        }

        public ConfiguratorDecorator(PluginConfiguration configuration) 
            : base(configuration)
        {            
        }

        /// <summary>
        /// тут можно указывать необходимые параметры для локализации, дополнительные настройки для Enum (в случае в PropertyGrid) и т.п.
        /// </summary>
        [Localizable(true)]
        public string FileName
        {
            get { return ((PluginConfiguration)Config).FileName; }
            set { ((PluginConfiguration)Config).FileName = value; }
        }
        
        public string SomeField
        {
            get { return ((PluginConfiguration)Config).SomeField; }
            set { ((PluginConfiguration)Config).SomeField = value; }
        }

        /// <summary>
        /// наименование
        /// </summary>
        public override string Name
        {
            get { return "PluginWithDecorator"; }
        }
    }
}
