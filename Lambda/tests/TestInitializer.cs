using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sparc.Hardware.CommonUnit.Test;

namespace Sparc.Lambda.Test
{
    [TestClass]
    public class TestInitializer
    {
        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context)
        {
            ConfigurationProvider.Current = DefaultConfigurationProvider.Instance;
        }
    }
}
