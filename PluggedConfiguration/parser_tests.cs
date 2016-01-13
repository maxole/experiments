using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PluggedConfiguration
{
    [TestClass]
// ReSharper disable InconsistentNaming
    public class parser_tests
// ReSharper restore InconsistentNaming
    {
        private IParserAgent _agent;

        [TestInitialize]
        public void SetUp()
        {
            var container = new Container();
            container.Configure();

            _agent = container.GetInstance<IParserAgent>();
            _agent.Initialize();
        }

        [TestMethod]
        public void get_parser_user()
        {            
            var parser = _agent.GetParserUser("1");
            Assert.IsInstanceOfType(parser, typeof(ParserUser));
        }

        [TestMethod]
        public void get_default_parser()
        {
            var parser = _agent.GetParserUser();
            Assert.IsInstanceOfType(parser, typeof(ParserDefault));
        }

        [TestMethod]
        public void get_custom_parser()
        {
            var parser = _agent.GetParserUser("2");            
            Assert.AreEqual(2, parser.Parser.Id);
        }
    }
}