using System.Drawing;

namespace OCR
{
    public interface ILearnedImage : IExportedImage
    {
        string[] LearnedData { get; }
    }

    public class LearnedImage : ILearnedImage
    {
        public LearnedImage(IExportedImage exported, string[] data)
        {
            Image = exported.Image;
            Blobs = new IBlob[exported.Blobs.Length];
            exported.Blobs.CopyTo(Blobs, 0);
            ExportData = exported.ExportData;
            LearnedData = data;
        }

        public Bitmap Image { get; private set; }
        public IBlob[] Blobs { get; private set; }
        public string ExportData { get; private set; }
        public string[] LearnedData { get; private set; }
    }
}