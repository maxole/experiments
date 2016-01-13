namespace PluggedConfiguration
{
    public interface IParserAgent
    {
        void Initialize();
        IParserUser GetParserUser(string instance);
        IParserUser GetParserUser();
    }

    public class ParserAgent : IParserAgent
    {
        private readonly IObjectFactory _factory;
        private readonly ParserConfiguration _configurations;

        public ParserAgent(IObjectFactory factory)
        {
            _factory = factory;
            _configurations = _factory.GetInstance<IConfigurationManager>().ParserConfiguration;
        }

        public void Initialize()
        {
            foreach (var item in _configurations.Items)
            {
                var configuration = item;
                _factory.Configure(configure => configure.For<IParser>().Use(new Parser(configuration)).Named(configuration.Instance));
            }
        }

        public IParserUser GetParserUser(string instance)
        {
            var parser = _factory.GetInstance<IParser>(instance);
            return new ParserUser(parser);
        }

        public IParserUser GetParserUser()
        {
            var parser = _factory.GetInstance<IParser>();
            return new ParserDefault(parser);
        }
    }
}