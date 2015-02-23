using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

// ReSharper disable once CheckNamespace
namespace Core.Entity
{
    using Serializer;

    public interface IEntity : IXmlSerializable
    {
        Guid SystemId { get; set; }
    }

    public abstract class TestDataDetailsBase : IEntity
    {
        protected TestDataDetailsBase()
        {
            SystemId = Guid.NewGuid();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            XmlDataSerializer.Read(this, reader);
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlDataSerializer.Write(this, writer);
        }

        [XmlElement]
        public Guid SystemId { get; set; }
    }

    public abstract class VerificationBase : List<TestDataDetailsBase>, IEntity
    {
        [XmlElement]
        public string Name { get; set; }

        protected VerificationBase()
        {
            SystemId = Guid.NewGuid();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            XmlDataSerializer.Read(this, reader);
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlDataSerializer.Write(this, writer);
        }

        [XmlElement]
        public Guid SystemId { get; set; }
    }

    public abstract class TestDataBase : List<VerificationBase>, IEntity
    {
        protected TestDataBase()
        {
            SystemId = Guid.NewGuid();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            XmlDataSerializer.Read(this, reader);
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlDataSerializer.Write(this, writer);
        }

        [XmlElement]
        public Guid SystemId { get; set; }
    }
}