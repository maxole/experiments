using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

// ReSharper disable once CheckNamespace
namespace Core.Repository
{
    using Entity;
    using Serializer;

    public interface IDataRepository
    {
        IEnumerable<T> Fetch<T>() where T : IEntity;
        void Save<T>(T data) where T : IEntity;
        void Delete<T>(T data) where T : IEntity;
        void DeleteAll();
    }

    public class DataRepository : IDataRepository
    {
        private readonly Encoding _encoding = Encoding.UTF8;

        public IEnumerable<T> Fetch<T>() where T : IEntity
        {
            var path = GetDirectory();
            return Directory.GetFiles(path, "*.xml").Select(file =>
            {
                var content = File.ReadAllText(file, _encoding);
                return new StringBuilder(content);
            }).Select(builder => new XmlDataSerializer().Deserialize<T>(builder));
        }

        public void Save<T>(T data) where T : IEntity
        {         
            Delete(data);
            var xml = new XmlDataSerializer().Serialize(data);            
            File.WriteAllText(BuildFileName(data), xml.ToString(), _encoding);
        }

        public void Delete<T>(T data) where T : IEntity
        {
            var path = BuildFileName(data);
            if (File.Exists(path))
                File.Delete(path);
        }

        public void DeleteAll()
        {
            var path = GetDirectory();
            Directory.Delete(path, true);
        }

        private string GetDirectory()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "data");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            return path;
        }

        private string BuildFileName(IEntity data)
        {
            var path = GetDirectory();
            return Path.Combine(path, data.SystemId + ".xml");            
        }
    }
}
