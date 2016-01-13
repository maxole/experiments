using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using AForge.Imaging;
using AForge.Imaging.Filters;

namespace OCR.Scanning
{
    public interface IScanner
    {
        IScanImage Scan(ISource source, ScannerOptions options);
    }

    public class Scanner : IScanner
    {
        public IScanImage Scan(ISource source, ScannerOptions options)
        {
            var image = new Bitmap(source.Take());

            // create grayscale filter (BT709)
            var filter = new Grayscale(0.2125, 0.7154, 0.0721);
            var mBinarized = filter.Apply(image);

            // Binarize Picture.            
            var bin = new Threshold(options.Threshold);
            bin.ApplyInPlace(mBinarized);

            // create filter
            var inv = new Invert();
            inv.ApplyInPlace(mBinarized);

            // create an instance of blob counter algorithm
            var bc = new BlobCounter { ObjectsOrder = ObjectsOrder.XY };
            bc.ProcessImage(mBinarized);
            var blobsRect = bc.GetObjectsRectangles();

            var b = Order(blobsRect);

            return new ScanImage(image, b);
        }

        private IBlob[] Order(Rectangle[] data)
        {
            if (data.Length < 1)
                return Enumerable.Empty<IBlob>().ToArray();

            var blobs = data.OrderBy(x => x.Y).ToArray();

            return (from reordered in RowingBlobs(blobs) 
                    from rectangle in reordered.Value 
                    select rectangle.CreateBlob(reordered.Key)).ToArray();
        }

        /// <summary>
        /// This method tries to order the blobs in rows.
        /// </summary>
        private Dictionary<int, List<Rectangle>> RowingBlobs(Rectangle[] blobs)
        {
            var result = new Dictionary<int, List<Rectangle>>();

            // Add the first row and blob.
            var currRowInd = 0;
            result.Add(currRowInd, new List<Rectangle>());
            result[currRowInd].Add(blobs[0]);

            // Now we loop thru all the blobs and try to guess where a new line begins.
            for (var i = 1; i < blobs.Length; i++)
            {
                // Since the blobs are ordered by Y, we consider a NEW line if the current blob's Y
                // is BELOW the previous blob lower quarter.
                // The assumption is that blobs on the same row will have more-or-less same Y, so if
                // the Y is below the previous blob lower quarter it's probably a new line.
                if (blobs[i].Y > blobs[i - 1].Y + 0.75 * blobs[i - 1].Height)
                {
                    // Add a new row to the dictionary
                    ++currRowInd;
                    result.Add(currRowInd, new List<Rectangle>());
                }

                // Add blob to the current row.
                result[currRowInd].Add(blobs[i]);
            }

            return result;
        }
    }
}