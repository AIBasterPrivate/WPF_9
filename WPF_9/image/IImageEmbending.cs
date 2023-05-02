using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace WPF_9.image
{
    internal interface IImageEmbending
    {
        Bitmap Embending(IARGBImage image,byte[] data);
        byte[] UnEmbending(IARGBImage image);
        

    }
}
