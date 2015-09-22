using System;
using OCR.Export;
using OCR.Learn;
using OCR.Prediction;
using OCR.Scanning;

namespace OCR.Preprocess
{
    public interface IProcessor
    {
        IScanImage Scan(IScanner scanner, Action<IScannerConfiguration> configuration);
        IExportedImage Export(IExporter exporter, Action<IExportConfiguration> configuration);
        ILearnedImage Learn(ILearner learner, Action<ILearnerConfiguration> configuration);
        IPredictImage Predict(IPredict predict, Action<IPredictConfiguration> configuration);
    }

    public class Processor : IProcessor
    {
        private IScanImage _scanImage;
        private IExportedImage _exportedImage;
        private ILearnedImage _learnedImage;

        public IScanImage Scan(IScanner scanner, Action<IScannerConfiguration> configuration)
        {            
            ScannerConfiguration scannerConfiguration;
            configuration(scannerConfiguration = new ScannerConfiguration(scanner));

            _scanImage = scannerConfiguration.Scan(scannerConfiguration.Source, scannerConfiguration.Options);
            return _scanImage;
        }

        public IExportedImage Export(IExporter exporter, Action<IExportConfiguration> configuration)
        {
            ExportConfiguration exportConfiguration;
            configuration(exportConfiguration = new ExportConfiguration(exporter));

            _exportedImage = exportConfiguration.Export(_scanImage, exportConfiguration.Options);
            return _exportedImage;
        }

        public ILearnedImage Learn(ILearner learner, Action<ILearnerConfiguration> configuration)
        {
            LearnerConfiguration learnerConfiguration;
            configuration(learnerConfiguration = new LearnerConfiguration(learner));

            _learnedImage = learnerConfiguration.Learn(_exportedImage, learnerConfiguration.Alphabet, learnerConfiguration.Options);
            return _learnedImage;
        }

        public IPredictImage Predict(IPredict predict, Action<IPredictConfiguration> configuration)
        {
            PredictConfiguration predictConfiguration;
            configuration(predictConfiguration = new PredictConfiguration(predict));
            return predictConfiguration.Predict(_scanImage, predictConfiguration.Alphabet, predictConfiguration.Options);
        }        
    }
}