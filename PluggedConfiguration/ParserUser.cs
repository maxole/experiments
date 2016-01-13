namespace PluggedConfiguration
{
    public interface IParserUser
    {
        IParser Parser { get; }
    }

    public class ParserUser : IParserUser
    {
        public IParser Parser { get; private set; }

        public ParserUser(/*[ParserConfigurationAttribute("default")]*/ IParser parser)
        {
            Parser = parser;
        }
    }

    public class ParserDefault : IParserUser
    {
        public IParser Parser { get; private set; }

        public ParserDefault(IParser parser)
        {
            Parser = parser;
        }        
    }
}