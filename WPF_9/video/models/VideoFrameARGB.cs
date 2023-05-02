using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_9.video.models
{
    internal class VideoFrameARGB : IImageEmbending
    {
        public Bitmap Image { get; private set; }

        public VideoFrameARGB(Bitmap image)
        {
            Image = image;
        }

        public Color GetPixel(int x, int y)
        {
            return Image.GetPixel(x, y);
        }

        public void SetPixel(int x, int y, Color color)
        {
            Image.SetPixel(x, y, color);
        }

        public Bitmap GetBitmap()
        {
            return Image;
        }

        public Bitmap GetCopyBitmap()
        {
            return new Bitmap(Image);
        }
    }
}
