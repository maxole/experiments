using System;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace EfawateerGateway
{
    public interface ISerializer
    {
        string Serialize<T>(T data);
        T Deserialize<T>(XElement data);
    }

    public class Serializer : ISerializer
    {
        private readonly IXmlParameterOverriders _overriders;
        private readonly XmlSerializerNamespaces _namespaces;

        public Serializer(IXmlParameterOverriders overriders)
        {
            _overriders = overriders;

            _namespaces = new XmlSerializerNamespaces(new[]
            {
                new XmlQualifiedName(string.Empty, string.Empty)
            });
        }

        public string Serialize<T>(T data)
        {
            var serializer = GetSerializer<T>();            
            var buffer = new StringBuilder();
            var writer = XmlWriter.Create(buffer, new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                CloseOutput = true
            });

            serializer.Serialize(writer, data, _namespaces);

            return buffer.ToString();
        }

        public T Deserialize<T>(XElement data)
        {
            var serializer = GetSerializer<T>();
            var reader = data.CreateReader();
            return (T) serializer.Deserialize(reader);
        }

        private XmlSerializer GetSerializer<T>()
        {
            XmlSerializer serializer;
            var get = _overriders.Overriders.TryGetValue(typeof(T).Name, out serializer);
            if (!get)
                throw new Exception("Не найден сериализатор для типа " + typeof(T).Name);
            return serializer;
        }
    }
}