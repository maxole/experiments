using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using ConfiguratorDecorator;

namespace ConfiguratorManager
{
    /// <summary>
    /// менеджер конфигураций
    /// </summary>
    public interface IConfigurationManager
    {
        /// <summary>
        /// выполнить конфигурирование
        /// </summary>
        void Configure();
        /// <summary>
        /// получить зарегистрированные декораторы
        /// </summary>
        /// <remarks>
        /// колелкция в виде
        /// тип настройки,
        /// декоратор настройки
        /// </remarks>
        /// <returns></returns>
        Dictionary<Type, IConfiguratorDecorator> GetDecorators();
        /// <summary>
        /// сохранить настройки
        /// </summary>
        void Save();
        /// <summary>
        /// получить заданные настройки
        /// </summary>
        /// <remarks>
        /// бросается исключением если настройки не найдены
        /// </remarks>
        /// <typeparam name="T">параметр тип настроек</typeparam>
        /// <returns>экземпляр настроек</returns>
        T GetCustomConfig<T>() where T : class;
        /// <summary>
        /// получить заданные настройки
        /// </summary>
        /// <remarks>
        /// возвращает null в случае если настройки не найдены
        /// </remarks>
        /// <typeparam name="T">параметр тип настроек</typeparam>
        /// <returns>экземпляр настроек, null в других случаях</returns>
        T TryGetCustomConfig<T>() where T : class;
    }
    /// <summary>
    /// менеджер конфигураций app.config файлов
    /// </summary>    
    public class FileConfigurationManager : IConfigurationManager
    {
        /// <summary>
        /// конфигурации в файле app.config
        /// </summary>
        private Configuration _fileConfiguration;
        /// <summary>
        /// найстройки прочитанные из файла app.config
        /// </summary>
        private readonly Dictionary<Type, object> _customConfiguration = new Dictionary<Type, object>();
        /// <summary>
        /// декораторы
        /// </summary>
        private readonly Dictionary<Type, IConfiguratorDecorator> _decorators = new Dictionary<Type, IConfiguratorDecorator>();

        /// <summary>
        /// выполнить конфигурирование
        /// </summary>
        public void Configure()
        {
            _fileConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // получить все типы поддерживающие настройки
            var types = GetTypesWithConfiguration();

            // создать значения по умолчанию
            foreach (var type in types)
                _customConfiguration[type] = Activator.CreateInstance(type);

            // обновить данные, если они были найдены в конфиге
            foreach (var section in _fileConfiguration.Sections)
            {
                var type = section.GetType();
                if (!types.Contains(type))
                    continue;
                _customConfiguration[type] = section;
            }

            // получить типы-декораторы
            var decorators = GetTypesWithDecorators();

            // регистрация и создание декораторов
            foreach (var type in types)
            {
                var innerType = type;
                // найти декторатора которые поддерживает данный тип конфигурации
                var decorator = decorators.SingleOrDefault(d => d.BaseType != null && d.BaseType.GetGenericArguments().Contains(innerType));
                if (decorator == null)
                    continue;

                // декоратор найден, создаем его
                var configuratorDecorator = (IConfiguratorDecorator)Activator.CreateInstance(decorator, _customConfiguration[type]);                

                _decorators.Add(type, configuratorDecorator);
            }
        }
        /// <summary>
        /// получить зарегистрированные декораторы
        /// </summary>
        /// <returns></returns>
        public Dictionary<Type, IConfiguratorDecorator> GetDecorators()
        {
            // по идеи лучше возвращать что то типа ReadOnlyCollection
            return _decorators;
        }
        /// <summary>
        /// сохранить настройки
        /// </summary>
        public void Save()
        {
            foreach (var o in _customConfiguration)
            {
                var section = _fileConfiguration.Sections[o.Key.FullName];
                if (section == null)
                {
                    section = (ConfigurationSection)o.Value;
                    _fileConfiguration.Sections.Add(o.Key.FullName, section);
                }
                else
                    section = (ConfigurationSection)o.Value;
                section.SectionInformation.ForceSave = true;
            }

            _fileConfiguration.Save(ConfigurationSaveMode.Modified);
        }
        /// <summary>
        /// получить заданные настройки
        /// </summary>
        /// <remarks>
        /// бросается исключением если настройки не найдены
        /// </remarks>
        /// <typeparam name="T">параметр тип настроек</typeparam>
        /// <returns>экземпляр настроек</returns>
        public T GetCustomConfig<T>() where T : class
        {
            var customConfig = TryGetCustomConfig<T>();
            if (customConfig != null)
                return customConfig;

            throw new InvalidOperationException(
                string.Format("Unknown configuration: {0}.", typeof(T).Name));
        }
        /// <summary>
        /// получить заданные настройки
        /// </summary>
        /// <remarks>
        /// возвращает null в случае если настройки не найдены
        /// </remarks>
        /// <typeparam name="T">параметр тип настроек</typeparam>
        /// <returns>экземпляр настроек, null в других случаях</returns>
        public T TryGetCustomConfig<T>() where T : class
        {
            return _customConfiguration.ContainsKey(typeof(T))
                ? (T)_customConfiguration[typeof(T)]
                : null;
        }

        /// <summary>
        /// получить все типы поддерживающие настройки
        /// </summary>
        /// <returns></returns>
        private List<Type> GetTypesWithConfiguration()
        {
            var assemblies = LoadAssembliesFromFiles(GetFilesFromBaseDirectory());
            return assemblies.SelectMany(a => a.GetTypes()).Where(t => typeof (ConfigurationSection).IsAssignableFrom(t) && t != typeof (ConfigurationSection)).ToList();
        }
        /// <summary>
        /// получить типы-декораторы
        /// </summary>
        /// <returns></returns>
        private List<Type> GetTypesWithDecorators()
        {
            var assemblies = LoadAssembliesFromFiles(GetFilesFromBaseDirectory());
            return assemblies.SelectMany(a => a.GetTypes()).Where(t => typeof(IConfiguratorDecorator).IsAssignableFrom(t) && t != typeof(IConfiguratorDecorator) && !t.IsAbstract).ToList();
        }

        private IEnumerable<Assembly> LoadAssembliesFromFiles(IEnumerable<string> assemblyFiles)
        {
            return assemblyFiles
                .Select(LoadAssemblyFromFile)
                .Where(assembly => (assembly != null))
                .ToList();
        }

        private static IEnumerable<string> GetFilesFromBaseDirectory()
        {
// ReSharper disable PossibleNullReferenceException
            return Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory).Where(
                file => Path.GetExtension(file).Equals(".exe", StringComparison.OrdinalIgnoreCase) ||
                Path.GetExtension(file).Equals(".dll", StringComparison.OrdinalIgnoreCase));
// ReSharper restore PossibleNullReferenceException
        }

        private static Assembly LoadAssemblyFromFile(string assemblyFile)
        {
            var assembly = default(Assembly);
            try
            {
                assembly = Assembly.LoadFrom(assemblyFile);
            }
// ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
            return assembly;
        }
    }
}
