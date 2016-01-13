namespace PluggedConfiguration
{
    public interface IConfigurationManager
    {
        void Initialize();
        ParserConfiguration ParserConfiguration { get; }
    }

    public class ConfigurationManager : IConfigurationManager
    {
        public ParserConfiguration ParserConfiguration { get; private set; }

        public void Initialize()
        {
            ParserConfiguration = new ParserConfiguration();
        }
    }
}