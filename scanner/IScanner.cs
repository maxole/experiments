using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace NS.Loader
{
    /// <summary>
    /// отсканированная коллекция параметров
    /// </summary>
    public interface IScanned
    {
        /// <summary>
        /// прочитанные параметры
        /// </summary>
        List<ParameterModel> Collection { get; }
    }
    /// <summary>
    /// интфейс сканирования параметров
    /// </summary>
    public interface IScanner : IScanned
    {
        /// <summary>
        /// выполняет сканирование параметров проверки из файла ресурсов с использованием переданного валидатора
        /// </summary>
        void Scan(ISource source, Action<IScannerValidatorConfiguration> validator);
    }

    public class Scanner : IScanner
    {
        private ScannerValidatorConfiguration _validatorConfiguration;

        public Scanner()
        {
            Collection = new List<ParameterModel>();
        }

        public void Scan(ISource source, Action<IScannerValidatorConfiguration> validator)
        {
            Collection.Clear();
            validator(_validatorConfiguration = new ScannerValidatorConfiguration());
            Load(source);
        }

        private void Load(ISource source)
        {
            var root = source.Take();
            var document = XDocument.Parse(root);

            var serializer = new XmlSerializer(typeof (ParameterModel));
            foreach (var data in document.Root.Elements("parameter")
                .Select(element => element.ToString())
                .Where(value => _validatorConfiguration.Verify(value)))
                using (var reader = new StringReader(data))
                {
                    var model = (ParameterModel) serializer.Deserialize(reader);
                    Collection.Add(model);
                }
        }

        public List<ParameterModel> Collection { get; private set; }
    }
}