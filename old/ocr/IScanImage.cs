using System.Drawing;

namespace OCR
{
    public interface IScanImage
    {
        Bitmap Image { get; }
        IBlob[] Blobs { get; }
    }

    public class ScanImage : IScanImage
    {
        public Bitmap Image { get; private set; }
        public IBlob[] Blobs { get; private set; }

        public ScanImage(Bitmap image, IBlob[] blobs)
        {
            Image = image;
            Blobs = blobs;
        }
    }
}