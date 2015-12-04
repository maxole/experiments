using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using Core.Attributes;

namespace Core.Scanner
{
    public interface IScanner
    {
        /// <summary>
        /// выполняет сканирование параметров проверки из файла ресурсов с использованием переданного валидатора
        /// </summary>
        IList<T> Scan<T>(ISource source, Action<IScannerValidatorConfiguration> validator);
    }

    public class Scanner : IScanner
    {
        private readonly IXmlParameterOverriders _overriders;
        private ScannerValidatorConfiguration _configuration;

        public Scanner(IXmlParameterOverriders overriders)
        {
            _overriders = overriders;
        }

        public IList<T> Scan<T>(ISource source, Action<IScannerValidatorConfiguration> validator)
        {
            validator(_configuration = new ScannerValidatorConfiguration());

            var records = new List<T>();

            var root = source.Take();
            var document = XDocument.Parse(root);

            foreach (var data in document.Root.Elements().Select(element => new
            {
                type = element.Name.LocalName,
                element = element.ToString()
            }).Where(value => _configuration.Verify(value.element, source.Schema())))
            {
                using (var reader = new StringReader(data.element))
                {
                    XmlSerializer serializer;
                    _overriders.Overriders.TryGetValue(data.type, out serializer);
                    if (serializer == null)
                        throw new Exception("Неизвестный тип сериализатора для параметра " + data.type);

                    var model = (T)serializer.Deserialize(reader);

                    records.Add(model);
                }
            }

            return records;
        }
    }
}