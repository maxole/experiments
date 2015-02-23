using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

// ReSharper disable once CheckNamespace
namespace Core.Serializer
{
    using Entity;

    /// <summary>
    /// сериализатор тестовых данных
    /// </summary>
    public interface IDataSerializer
    {
        /// <summary>
        /// преобразовать в XML
        /// </summary>
        /// <param name="data">данные</param>
        /// <returns>объект, содержащий сериализованные данные</returns>
        StringBuilder Serialize<T>(T data) where T :IEntity;

        /// <summary>
        /// преобразовать из XML в объект
        /// </summary>
        /// <typeparam name="T">тип параметр</typeparam>
        /// <param name="builder">XML данные</param>
        /// <returns>сериализованный объект</returns>
        T Deserialize<T>(StringBuilder builder) where T : IEntity;
    }

    /// <summary>
    /// сериализатор тестовых данных
    /// </summary>
    public class XmlDataSerializer : IDataSerializer
    {
        /// <summary>
        /// преобразовать в XML
        /// </summary>
        /// <param name="data">данные</param>
        /// <returns>объект, содержащий сериализованные данные</returns>
        public StringBuilder Serialize<T>(T data) where T : IEntity
        {
            var builder = new StringBuilder();
            using (var writer = new StringWriter(builder))
            {
                var type = data.GetType();
                var serializer = new XmlSerializer(type, type.AssemblyQualifiedName);
                serializer.Serialize(writer, data);
            }            
            return builder;
        }
        /// <summary>
        /// преобразовать из XML в объект
        /// </summary>
        /// <typeparam name="T">тип параметр</typeparam>
        /// <param name="builder">XML данные</param>
        /// <returns>сериализованный объект</returns>
        public T Deserialize<T>(StringBuilder builder) where T : IEntity
        {
            using (var reader = new StringReader(builder.ToString()))
            {
                var serializer = new XmlSerializer(typeof(T), typeof(T).AssemblyQualifiedName);
                return (T)serializer.Deserialize(reader);
            }
        }

        public bool CanDeserialize<T>(StringBuilder builder)
        {
            using (var reader = new StringReader(builder.ToString()))
            {
                var document = XDocument.Load(reader);
                if (document.Root == null)
                    throw new Exception("error in reader xml");

                if (!document.Root.HasElements)
                    return false;
                var element = document.Root;
                if (element == null)
                    return false;

                var typeName = element.FirstAttribute.Value;
                var type = Type.GetType(typeName);
                if (type == null)
                    return false;

                return type == typeof (T);
            }
        }

        public static void Read(IEntity component, XmlReader reader)
        {
            var document = XDocument.Load(reader);
            if (document.Root == null)
                throw new Exception("error in reader xml");

            foreach (var element in document.Root.Elements())
            {
                var typeName = element.FirstAttribute.Value;
                var type = Type.GetType(typeName);
                if (type == null)
                    throw new Exception("type is null");
                if (!type.Assembly.GlobalAssemblyCache)
                {
                    var serializer = new XmlSerializer(type);
                    var e = new XElement(element.Name.LocalName, element.Elements());
                    var o = serializer.Deserialize(e.CreateReader());
                    var list = (component as IList);
                    if (list != null)
                        list.Add(o);
                }
                else
                {
                    var property = component.GetType().GetProperty(element.Name.LocalName);
                    var value =
                        TypeDescriptor.GetConverter(property.PropertyType).ConvertFromInvariantString(element.Value);
                    if (property.CanWrite)
                        property.SetValue(component, value, null);
                    /*
                    component.GetType()
                        .InvokeMember(property.Name,
                            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.SetProperty |
                            BindingFlags.Instance,
                            null, component, new object[] {value});
                    */
                }
            }
        }

        public static void Write(IEntity component, XmlWriter writer)
        {
            var type = component.GetType();
            foreach (var property in type
                .GetProperties()
                .Where(p => p.GetCustomAttributes(typeof(XmlElementAttribute), true).Any()))
            {
                var tmp = property.GetValue(component, null);
                if (tmp == null)
                    continue;
                var value = tmp.ToString();
                writer.WriteElementString("i", property.Name, property.PropertyType.FullName, value);
            }

            var enumerable = (component as IEnumerable);
            if (enumerable == null)
                return;
            foreach (var r in enumerable)
            {
                var serializer = new XmlSerializer(r.GetType(), r.GetType().AssemblyQualifiedName);
                serializer.Serialize(writer, r);
            }
        }
    }
}