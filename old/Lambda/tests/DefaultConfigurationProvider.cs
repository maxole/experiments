using System;
using System.Collections.Generic;
using Core.Configuration;
using Rhino.Mocks;
using Sparc.Hardware.CommonUnit.Test;

namespace Sparc.Lambda.Test
{
    class DefaultConfigurationProvider : ConfigurationProvider
    {
        private static DefaultConfigurationProvider _instance = new DefaultConfigurationProvider();
        private AppConfigurationManager _manager;

        private DefaultConfigurationProvider()
        {
            var resolver = MockRepository.GenerateMock<IConfigurationAssemblyResolver>();
            resolver.Stub(r => r.GetTypesWithConfiguration()).Return(new List<Type>
            {
                typeof (TransportConfig),
                typeof (Config)
            });

            _manager = new AppConfigurationManager(resolver);
            _manager.Configure();

            //_manager.GetCustomConfig<TransportConfig>();
            _manager.GetCustomConfig<Config>().Idn = "LAMBDA,GENH30-25";
            _manager.GetCustomConfig<Config>().Voltage = 27.0f;
            _manager.GetCustomConfig<Config>().Current = 6.0f;
        }

        public override IConfigurationManager Configuration
        {
            get { return _manager; }
        }

        protected override ConfigurationProvider Reset()
        {
            _instance = new DefaultConfigurationProvider();
            return _instance;
        }

        public static DefaultConfigurationProvider Instance
        {
            get { return _instance; }
        }
    }
}
