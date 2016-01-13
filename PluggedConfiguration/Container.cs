using System;
using System.Linq;
using System.Reflection;
using StructureMap;
using StructureMap.Pipeline;

namespace PluggedConfiguration
{
    public interface IObjectFactory
    {
        void Configure(Action<ConfigurationExpression> configuration);
        T GetInstance<T>();
        T GetInstance<T>(string name);
    }

    public class Container : IObjectFactory
    {
        private readonly StructureMap.Container _container;

        public Container()
        {
            _container = new StructureMap.Container();
            _container.Configure(x => x.For<IObjectFactory>().Use(this));
        }

        public void Configure(Action<ConfigurationExpression> configuration)
        {
            _container.Configure(configuration);
        }

        public T GetInstance<T>()
        {
            return _container.GetInstance<T>();
        }

        public T GetInstance<T>(string name)
        {
            return _container.GetInstance<T>(name);
        }

        public void Configure()
        {
            _container.Configure(configure =>
            {
                configure.For<IConfigurationManager>().Singleton().Use<ConfigurationManager>();
                configure.For<IParserAgent>().Singleton().Use<ParserAgent>();
            });

            _container.GetInstance<IConfigurationManager>().Initialize();

            ParserConfigurationAttributes();
        }

        private void ParserConfigurationAttributes()
        {
            var enumerable = _container
                .Model
                .AllInstances
                .Where(iref => iref != null)
                .Select(iref => iref.GetType()
                    .GetProperty("Instance", BindingFlags.Public | BindingFlags.Instance)
                    .GetValue(iref, null));

            var configuredInstances = enumerable
                .OfType<IConfiguredInstance>()
                .Where(ci => ci.PluggedType.GetConstructor() != null);

            configuredInstances
                .SelectMany(ci => ci.PluggedType.GetConstructor()
                    .GetParameters()
                    .Where(pi => pi.IsDefined(typeof (ParserConfigurationAttribute), false))
                    .Select(pi => new
                    {
                        instance = ci,
                        parameter = pi,
                        attribute = pi
                            .GetCustomAttributes(typeof (ParserConfigurationAttribute), false)
                            .Cast<ParserConfigurationAttribute>()
                            .Single()
                    }))
                .ToList()
                .ForEach(x =>
                {
                    x.instance.Dependencies.AddForConstructorParameter(x.parameter, new ReferencedInstance(x.attribute.Name));
                    //x.instance.Dependencies.AddForConstructorParameter(x.parameter, _container.GetInstance<ParserConfigurationItem>(x.attribute.Name));
                });
        }
    }

    public static class Foo
    {
        public static ConstructorInfo GetConstructor(this Type pluggedType)
        {
            var constructorInfo1 = (ConstructorInfo)null;
            foreach (ConstructorInfo constructorInfo2 in pluggedType.GetConstructors())
            {
                if (constructorInfo1 == null)
                    constructorInfo1 = constructorInfo2;
                else if (constructorInfo2.GetParameters().Length > constructorInfo1.GetParameters().Length)
                    constructorInfo1 = constructorInfo2;
            }
            return constructorInfo1;
        }
    }
}
