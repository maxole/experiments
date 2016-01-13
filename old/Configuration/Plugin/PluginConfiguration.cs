using System.Configuration;

namespace Plugin
{
    /// <summary>
    /// пример конфигурации взят из мсдн
    /// </summary>
    /// <remarks>
    /// допустим плагин использует данные настройки
    /// </remarks>
    public class PluginConfiguration : ConfigurationSection
    {
        // The collection (property bag) that contains 
        // the section properties.
        private static ConfigurationPropertyCollection _properties;

        // The FileName property.
        private static readonly ConfigurationProperty _FileName = new ConfigurationProperty("fileName", typeof(string), "default.txt", ConfigurationPropertyOptions.IsRequired);

        public PluginConfiguration()
        {
            _properties = new ConfigurationPropertyCollection();
            _properties.Add(_FileName);
        }

        [StringValidator(InvalidCharacters = " ~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public string FileName
        {
            get
            {
                return (string)this["fileName"];
            }
            set
            {
                // With this you disable the setting.
                // Remember that the _ReadOnly flag must
                // be set to true in the GetRuntimeObject.                
                this["fileName"] = value;
            }
        }

        // This is a key customization. 
        // It returns the initialized property bag.
        protected override ConfigurationPropertyCollection Properties
        {
            get
            {
                return _properties;
            }
        }
    }
}
