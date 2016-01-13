using System;

namespace OCR.Learn
{
    public interface ILearnerConfiguration
    {
        IAlphabet Alphabet { get; set; }
        LearnerOptions Options { get; set; }
        Func<IExportedImage, IAlphabet, LearnerOptions, ILearnedImage> Learn { get; set; }
    }

    public class LearnerConfiguration : ILearnerConfiguration
    {     
        public IAlphabet Alphabet { get; set; }
        public LearnerOptions Options { get; set; }
        public Func<IExportedImage, IAlphabet, LearnerOptions, ILearnedImage> Learn { get; set; }

        public LearnerConfiguration(ILearner learner)
        {
            Learn = learner.Learn;
        }
    }
}