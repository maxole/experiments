using System;

namespace OCR.Scanning
{
    public interface IScannerConfiguration
    {        
        ScannerOptions Options { get; set; }
        ISource Source { get; set; }
        Func<ISource, ScannerOptions, IScanImage> Scan { get; }
    }

    public class ScannerConfiguration : IScannerConfiguration
    {
        public ScannerOptions Options { get; set; }
        public ISource Source { get; set; }
        public Func<ISource, ScannerOptions, IScanImage> Scan { get; private set; }

        public ScannerConfiguration(IScanner scanner)
        {
            Scan = scanner.Scan;
        }
    }
}