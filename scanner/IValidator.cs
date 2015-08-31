using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace NS.Loader
{
    /// <summary>
    /// осуществляет валидацию параметров задачи 5
    /// </summary>
    public interface IValidator
    {
        /// <param name="data">данные</param>
        bool Verify(string data);
        /// <summary>
        /// ошибки валидирования
        /// </summary>
        StringBuilder Errors { get; }
    }
    /// <summary>
    /// задача валидатора проверить наличие параметров, проверить правильность параметров
    /// </summary>    
    public class Validator : IValidator
    {
        private string _schema;

        public Validator()
        {
            _schema = string.Empty;
            Errors = new StringBuilder();  
        }

        public bool Verify(string data)
        {
            var verify = true;
            if (!string.IsNullOrEmpty(data))
            {                
                if (string.IsNullOrEmpty(_schema))
                    _schema = GetValidateSchema();

                var schemas = new XmlSchemaSet();
                schemas.Add("", XmlReader.Create(new StringReader(_schema)));

                try
                {
                    var document = XDocument.Parse(data);
                    document.Validate(schemas, (sender, e) =>
                    {
                        Errors.AppendLine(e.Message);
                        verify = false;
                    });
                }
                catch (Exception exception)
                {
                    Errors.AppendLine("Ошибка в данных. " + exception.Message);
                    verify = false;
                }
            }
            else
                throw new Exception("Не заданы данные для проверки");
            return verify;
        }

        public StringBuilder Errors { get; private set; }

        private string GetValidateSchema()
        {
            var assembly = Assembly.GetExecutingAssembly();
            const string resourceName = "NS.resources.model.xsd";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}
