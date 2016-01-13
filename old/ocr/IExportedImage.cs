using System.Drawing;

namespace OCR
{
    public interface IExportedImage : IScanImage
    {
        string ExportData { get; }
    }

    public class ExportedImage : IExportedImage
    {
        public ExportedImage(IScanImage scanImage, string exportData)
        {
            ExportData = exportData;
            Image = scanImage.Image;
            Blobs = new IBlob[scanImage.Blobs.Length];
            scanImage.Blobs.CopyTo(Blobs, 0);
        }

        public Bitmap Image { get; private set; }
        public IBlob[] Blobs { get; private set; }
        public string ExportData { get; private set; }
    }
}