using System;

namespace NS.Loader
{
    /// <summary>
    /// конфигуратор валидатора параметров
    /// </summary>
    public interface IScannerValidatorConfiguration
    {
        IScannerValidatorConfiguration Validate(Predicate<string> verify);
    }

    public class ScannerValidatorConfiguration : IScannerValidatorConfiguration
    {
        public Predicate<string> Verify { get; private set; }

        public IScannerValidatorConfiguration Validate(Predicate<string> verify)
        {
            Verify = verify;
            return this;
        }
    }
}
