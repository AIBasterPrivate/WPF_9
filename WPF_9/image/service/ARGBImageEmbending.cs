using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Windows.Controls;

namespace WPF_9.image.service
{
    internal class ARGBImageEmbending : IImageEmbending
    {
        static readonly byte[] stopBytes = { 0, 0 };// same to '\0'

        public virtual Bitmap Embending(IARGBImage image, byte[] data)
        {
            CheckData(image, data);
            data = AddStopBytes(data);

            var copy = image.GetCopy();
            var pixelIndex = 0;
            var dataIndex = 0;

            for (int y = 0; y < copy.Height; y++)
            {
                for (int x = 0; x < copy.Width; x++)
                {
                    var pixel = copy.GetPixel(x, y);

                    if (pixelIndex < data.Length)
                    {
                        var databyte = data[dataIndex++];
                        Color newPixel = InsertDataToPixel(pixel, databyte);
                        copy.SetPixel(x, y, newPixel);
                    }

                    pixelIndex++;
                }
            }

            return copy;
        }

        public virtual byte[] UnEmbending(IARGBImage image)
        {
            CheckData(image);

            var data = new List<byte>();
            bool stopBytesFound = false;

            var bitmap = image.GetBitmap();
            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var extractedByte = SelectByteFromPixel(bitmap, y, x);
                    data.Add(extractedByte);

                    CheckIfStopBytesAreFound(data, ref stopBytesFound);
                    if (stopBytesFound)
                    { 
                        break;
                    }
                }

                if (stopBytesFound)
                { 
                    break;
                }
            }

            if (stopBytesFound)
            {
                RemoveStopBytes(data);
            }
            else
            {
                throw new Exception("Stop bytes missing");
            }

            return data.ToArray();
        }

        private static void RemoveStopBytes(List<byte> data)
        {
            data.RemoveRange(data.Count - stopBytes.Length, stopBytes.Length);
        }

        private void CheckIfStopBytesAreFound(List<byte> data,ref bool stopBytesFound)
        {
            if (data.Count >= stopBytes.Length &&
                        data[data.Count - stopBytes.Length] == stopBytes[0] &&
                        data[data.Count - 1] == stopBytes[1] &&
                        data.Count % 2 == 0)
            {
                stopBytesFound = true;
            }
        }

        public virtual byte SelectByteFromPixel( Bitmap bitmap, int y, int x)
        {
            int r = (bitmap.GetPixel(x, y).R & 0b111) << 5;
            int g = (bitmap.GetPixel(x, y).G & 0b11) << 3;
            int b = bitmap.GetPixel(x, y).B & 0b111;

            int dataByte = r | g | b;
            return (byte)dataByte;
        }

        private static byte[] AddStopBytes(byte[] data)
        {
            Array.Resize(ref data, data.Length + 2);
            data[data.Length - 2] = stopBytes[0];
            data[data.Length - 1] = stopBytes[1];
            return data;
        }

        public virtual Color InsertDataToPixel(Color pixel, byte databyte)
        {
            int r, g, b;
            r = (pixel.R & 0b11111000) | (databyte >> 5 & 0b111);
            g = ((pixel.G >> 2) << 2) | (databyte >> 3 & 0b11);
            b = ((pixel.B >> 3) << 3) | (databyte & 0b111);
            var newPixel = Color.FromArgb(pixel.A, r, g, b);
            return newPixel;
        }

        private static void CheckData(IARGBImage image, byte[] data)
        {
            CheckData(image);
            if (data == null) throw new ArgumentNullException("data is null");
            if (image.Width() * image.Height() < data.Length) throw new ArgumentException("Not enough place for embending");
        }

        private static void CheckData(IARGBImage image)
        {
            if (image == null) throw new ArgumentNullException("Image is null");
        }

    }
}
