using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace WPF_9.videos
{
    public interface IImageEmbending
    {
        void SetPixel(int x, int y, Color color);
        Color GetPixel(int x, int y);
        Bitmap GetBitmap();
        Bitmap GetCopyBitmap();
    }
}
