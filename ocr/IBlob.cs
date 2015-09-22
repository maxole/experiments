using System;
using System.Drawing;
using System.Drawing.Imaging;
using AForge.Imaging.Filters;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace OCR
{
    public interface IBlob : ICloneable
    {
        int Width { get; }
        int Height { get; }
        int Left { get; }
        int Top { get; }
        int RowIndex { get; }
        string Title { get; set; }
    }

    public class Blob : IBlob
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int RowIndex { get; set; }
        public string Title { get; set; }

        public object Clone()
        {
            return new Blob
            {
                Width = Width,
                Height = Height,
                Left = Left,
                Top = Top,
                RowIndex = RowIndex,
                Title = Title
            };
        }
    }

    public static class BlobExt
    {
        public static IBlob CreateBlob(this Rectangle rectangle, int row)
        {
            rectangle.Inflate(3, 3);
            var blob = new Blob
            {
                RowIndex = row,
                Top = rectangle.Y,
                Left = rectangle.X,
                Width = rectangle.Width,
                Height = rectangle.Height                
            };
            return blob;
        }

        public static Bitmap CropBlob(this IBlob blob, Image source, int extractedBackColor, int exportSize, int rotationAngel = 0)
        {
            // Create the target image, this is a squared image.
            var size = Math.Max(blob.Height, blob.Width);
            var newImage = new Bitmap(size, size, PixelFormat.Format24bppRgb);

            // Get the graphics object of the image.
            var g = Graphics.FromImage(newImage);

            // Create the background color to use (the image we create is larger than the blob (as we squared it)
            // so this would be the color of the excess areas.
            var bColor = Color.FromArgb(extractedBackColor, extractedBackColor, extractedBackColor);

            // Fill back color.
            g.FillRectangle(new SolidBrush(bColor), 0, 0, size, size);

            // Now we clip the blob from the PictureBox image.
            g.DrawImage(source, new Rectangle(0, 0, blob.Width, blob.Height), blob.Left, blob.Top, blob.Width, blob.Height, GraphicsUnit.Pixel);
            g.Dispose();

            if (rotationAngel != 0)
            {
                var filter = new RotateBilinear(rotationAngel, true) { FillColor = bColor };
                // apply the filter
                newImage = filter.Apply(newImage);
            }

            // Resize the image.
            var resizefilter = new ResizeBilinear(exportSize, exportSize);
            newImage = resizefilter.Apply(newImage);

            return newImage;
        }

        public static Vector GetPixels(this IBlob blob, Image image, int extractedBackColor, int exportSize)
        {
            // Get the blob image.
            Bitmap newImage = blob.CropBlob(image, extractedBackColor, exportSize);

            // Create the vector (Add the bias term).
            Vector xs = new DenseVector(newImage.Width * newImage.Height + 1);
            xs[0] = 1;

            // Loop thru the image pixels and add them to the vector.
            for (int i = 0; i < newImage.Height; i++)
            {
                for (int j = 0; j < newImage.Width; j++)
                {
                    Color pixel = newImage.GetPixel(i, j);
                    xs[1 + i * newImage.Width + j] = pixel.R;
                }
            }

            return xs;
        }
    }
}