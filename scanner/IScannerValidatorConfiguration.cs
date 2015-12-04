using System;

namespace Core.Scanner
{
    public interface IScannerValidatorConfiguration
    {
        IScannerValidatorConfiguration Validate(Func<string, string, bool> verify);
    }

    public class ScannerValidatorConfiguration : IScannerValidatorConfiguration
    {
        public Func<string, string, bool> Verify { get; private set; }

        public IScannerValidatorConfiguration Validate(Func<string, string, bool> verify)
        {
            Verify = verify;
            return this;
        }
    }
}