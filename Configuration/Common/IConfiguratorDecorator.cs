using System.ComponentModel;
using System.Configuration;

namespace ConfiguratorDecorator
{
    /// <summary>
    /// контракт декоратора для настроек
    /// </summary>
    public interface IConfiguratorDecorator
    {        
        /// <summary>
        /// конфигурация
        /// </summary>
        object Config { get; }
        /// <summary>
        /// наименование
        /// </summary>
        string Name { get; }
    }
    /// <summary>
    /// небольшой базовый класс для декораторов
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ConfiguratorDecorator<T> : IConfiguratorDecorator where T : ConfigurationSection
    {
        /// <summary>
        /// этот конструктор нужен для Activator.CreateInstance(decorator, null);
        /// в случае когда нет настроек
        /// </summary>
        protected ConfiguratorDecorator()
        {
            
        }

        protected ConfiguratorDecorator(T configuration)
        {
            Config = configuration;
        }

        /// <summary>
        /// конфигурация за которой смотрит декоратор
        /// </summary>
        [Browsable(false)] // эта опция не работает для wpf версии PropertyGrid
        public object Config { get; private set; }

        /// <summary>
        /// наименование
        /// </summary>
        [Browsable(false)] // эта опция не работает для wpf версии PropertyGrid
        public abstract string Name { get; }
    }    
}
