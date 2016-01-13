using System.ComponentModel;
using ConfiguratorDecorator;
using Plugin;

namespace SeparateDecorator
{
    /// <summary>
    /// декоратор для конкретных настроек
    /// </summary>
    public class DecoratorForPlugin : ConfiguratorDecorator<PluginConfiguration>
    {
        public DecoratorForPlugin()
            : base(new PluginConfiguration())
        {
        }

        public DecoratorForPlugin(PluginConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>
        /// тут можно указывать необходимые параметры для локализации, дополнительные настройки для Enum (в случае в PropertyGrid) и т.п.
        /// </summary>
        [DisplayName("display FileName")]
        [Category("category FileName")]
        [Description("description FileName")]
        public string FileName
        {
            get { return ((PluginConfiguration) Config).FileName; }
            set { ((PluginConfiguration) Config).FileName = value; }
        }

        /// <summary>
        /// наименование
        /// </summary>
        public override string Name
        {
            get { return "DecoratorForPlugin"; }
        }
    }
}
