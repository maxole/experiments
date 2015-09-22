using System;

namespace OCR.Prediction
{
    public interface IPredictConfiguration
    {
        IAlphabet Alphabet { get; set; }
        PredictOptions Options { get; set; }
        Func<IScanImage, IAlphabet, PredictOptions, IPredictImage> Predict { get; set; }
    }

    public class PredictConfiguration : IPredictConfiguration
    {
        public PredictOptions Options { get; set; }
        public Func<IScanImage, IAlphabet, PredictOptions, IPredictImage> Predict { get; set; }
        public IAlphabet Alphabet { get; set; }

        public PredictConfiguration(IPredict predict)
        {
            Predict = predict.PredictModel;
        }
    }
}