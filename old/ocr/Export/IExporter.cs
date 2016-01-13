using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace OCR.Export
{
    public interface IExporter
    {
        IExportedImage Export(IScanImage scanImage, ExporterOptions options);
    }

    public class Exporter : IExporter
    {
        public IExportedImage Export(IScanImage scanImage, ExporterOptions options)
        {           

            var export = new StringBuilder();

            using (var writer = new StringWriter(export))
            {
                // Group by RowIndex and select the group Left, Right, Top, Bottom
                foreach (var row in scanImage.Blobs.GroupBy(bp => bp.RowIndex))
                    foreach (var blob in row)
                    {
                        var key = row.Key + 1;
                        // Get the blob image.
                        var newImage = blob.CropBlob(scanImage.Image, options.ExtractedBackColor, options.ExportSize);
                        Write(writer, key, newImage);

                        // Write a rotated version.
                        newImage = blob.CropBlob(scanImage.Image, options.ExtractedBackColor, options.ExportSize, 10);
                        Write(writer, key, newImage);

                        // Write another rotated version (to the other dir).
                        newImage = blob.CropBlob(scanImage.Image, options.ExtractedBackColor, options.ExportSize, -10);
                        Write(writer, key, newImage);
                    }
                writer.Flush();
            }

            return new ExportedImage(scanImage, export.ToString());
        }            

        private static void Write(StringWriter writer, int key, Bitmap image)
        {
            // Loop thru all pixels and write them.
            for (var i = 0; i < image.Height; i++)
            {
                for (var j = 0; j < image.Width; j++)
                {
                    var pixel = image.GetPixel(i, j);
                    writer.Write(GetColorAverage(pixel) + ",");
                }
            }

            writer.WriteLine(key);
        }

        private static int GetColorAverage(Color color)
        {
            return (color.R + color.G + color.B) / 3;
        }
    }
}
