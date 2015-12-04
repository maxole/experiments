using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using Core.Attributes;

namespace Core.Scanner
{
    public interface IValidator
    {
        /// <param name="data">данные</param>
        /// <param name="schema">файл схема валидирования</param>
        bool Verify(string data, string schema);
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
        public Validator()
        {
            Errors = new StringBuilder();
        }

        public bool Verify(string data, string schema)
        {
            var verify = true;
            if (!string.IsNullOrEmpty(data))
            {
                var schemas = new XmlSchemaSet();
                schemas.Add("", XmlReader.Create(new StringReader(schema)));

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
    }
}