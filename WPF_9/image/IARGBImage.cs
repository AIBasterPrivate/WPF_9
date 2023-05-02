using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WPF_9.image
{
    public interface IARGBImage
    {
        int Width();
        int Height();
        Bitmap GetBitmap();
        void SetBitmap(Bitmap bitmap);
        void SetPixel(int x, int y, Color color);
        Color GetPixel(int x, int y);
        Bitmap GetCopy();

    }
}
