using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace OCR
{
    public interface IPredictImage
    {
        Image Image { get; }
        Dictionary<int, List<IBlob>> Collection { get; }
    }

    public class PredictImage : IPredictImage
    {
        public PredictImage(Image image, Dictionary<int, List<IBlob>> collection)
        {
            Image = image;
            Collection = collection;
        }

        public Image Image { get; private set; }
        public Dictionary<int, List<IBlob>> Collection { get; private set; }
    }

    public static class PredictImageExt
    {
        public static IEnumerable<IBlob> ToList(this IPredictImage predict)
        {
            return predict.Collection.Values.SelectMany(b => b);
        }
    }
}