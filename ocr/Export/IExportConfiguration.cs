using System;

namespace OCR.Export
{
    public interface IExportConfiguration
    {
        ExporterOptions Options { get; set; }
        Func<IScanImage, ExporterOptions, IExportedImage> Export { get; set; }
    }

    public class ExportConfiguration : IExportConfiguration
    {
        public ExporterOptions Options { get; set; }
        public Func<IScanImage, ExporterOptions, IExportedImage> Export { get; set; }

        public ExportConfiguration(IExporter exporter)
        {
            Export = exporter.Export;
        }
    }
}