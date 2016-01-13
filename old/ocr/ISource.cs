using System.Drawing;

namespace OCR
{
    public interface ISource
    {
        Image Take();
    }
}