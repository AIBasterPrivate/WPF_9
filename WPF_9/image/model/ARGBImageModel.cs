using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

namespace WPF_9.image.model
{
    internal class ARGBImageModel : IARGBImage
    {
        private Bitmap _bitmap;
        public ARGBImageModel(int width, int height)
        {
            CheckData(width, height);
            _bitmap = new Bitmap(width, height);
        }
        public ARGBImageModel(Bitmap bitmap) {
            CheckData(bitmap);
            _bitmap= bitmap;
        }

        public Bitmap GetBitmap()
        {
            return _bitmap;
        }

        public Color GetPixel(int x, int y)
        {
            CheckData(x,y);
            return _bitmap.GetPixel(x, y);
        }

        public int Height()
        {
            return _bitmap.Height;
        }

        public void SetPixel(int x, int y, Color color)
        {
            CheckData(x,y);
            _bitmap.SetPixel(x, y, color);
        }

        public int Width()
        {
            return _bitmap.Width;
        }
        
        public void SetBitmap(Bitmap bitmap)
        {
            CheckData(bitmap);
            _bitmap = bitmap;
        }

        private void CheckData(int width, int height)
        {
            if (width < 0 || height < 0)
            {
                throw new ArgumentException($"Values should be more than 0. Width: {width}. Height: {height}");
            }
        }

        private void CheckData(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentException("The bitmap is null");
            }
        }

        public Bitmap GetCopy()
        {
            return new Bitmap( _bitmap );
        }
    }
}
