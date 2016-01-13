namespace PluggedConfiguration
{
    public interface IParser
    {
        int Id { get; }
        string Name { get; }
    }

    public class Parser : IParser
    {
        private readonly ParserConfigurationItem _configuration;

        public Parser(ParserConfigurationItem configuration)
        {
            _configuration = configuration;
        }

        public int Id
        {
            get { return _configuration.Id; }
        }

        public string Name
        {
            get { return _configuration.Name; }
        }
    }
}
