using Accord.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using WPF_9.video.models;

namespace WPF_9.video.service
{
    public class FFmpegVideo_v2
    {
        static readonly byte[] stopBytes = { 0, 0 };// same to '\0'

        public static void Embending(string inPath, string outPath, byte[] data)
        {
            Array.Resize(ref data, data.Length + 2);
            data[data.Length - 2] = stopBytes[0];
            data[data.Length - 1] = stopBytes[1];

            using (var reader = new VideoFileReader())
            {
                reader.Open(inPath);

                int width = reader.Width;
                int height = reader.Height;
                int frameRate = (int)reader.FrameRate;

                using (var writer = new VideoFileWriter())
                {
                    writer.Open(outPath, width, height, frameRate, VideoCodec.Raw);

                    int pixelIndex = 0;
                    int dataIndex = 0;
                    for (int i = 0; i < reader.FrameCount; i++)
                    {
                        var frame = reader.ReadVideoFrame();
                        var bitmap = frame;
                        if (data.Length > dataIndex)
                        {
                            for (int y = 0; y < height; y++)
                            {
                                for (int x = 0; x < width; x++)
                                {
                                    var pixel = bitmap.GetPixel(x, y);

                                    if (pixelIndex < data.Length)
                                    {
                                        var databyte = data[dataIndex++];
                                        int r, g, b;
                                        r = (pixel.R & 0b11111000) | (databyte >> 5 & 0b111);
                                        g = ((pixel.G >> 2) << 2) | (databyte >> 3 & 0b11);
                                        b = ((pixel.B >> 3) << 3) | (databyte & 0b111);
                                        var newPixel = Color.FromArgb(pixel.A, r, g, b);

                                        bitmap.SetPixel(x, y, newPixel);
                                    }

                                    pixelIndex++;
                                }
                            }
                        }

                        writer.WriteVideoFrame(bitmap);
                    }

                    writer.Close();
                }

                reader.Close();
            }

        }

        public static byte[] UnEmbending(string path)
        {
            using (var reader = new VideoFileReader())
            {
                reader.Open(path);
                var data = new List<byte>();
                bool stopBytesFound = false;
                for (int i = 0; i < reader.FrameCount; i++)
                {
                    var frame = reader.ReadVideoFrame();
                    for (int y = 0; y < frame.Height; y++)
                    {
                        for (int x = 0; x < frame.Width; x++)
                        {
                            int r = (frame.GetPixel(x, y).R & 0b111) << 5;
                            int g = (frame.GetPixel(x, y).G & 0b11) << 3;
                            int b = frame.GetPixel(x, y).B & 0b111;

                            int dataByte = r | g | b;
                            data.Add((byte)dataByte);

                            if (data.Count >= stopBytes.Length &&
                                data[data.Count - stopBytes.Length] == stopBytes[0] &&
                                data[data.Count - 1] == stopBytes[1] &&
                                data.Count % 2 == 0)
                            {
                                stopBytesFound = true;
                                break;
                            }
                        }

                        if (stopBytesFound) break;
                    }

                    if (stopBytesFound) break;
                }
                reader.Close();

                if (stopBytesFound)
                {
                    data.RemoveRange(data.Count - stopBytes.Length, stopBytes.Length);
                }

                return data.ToArray();
            }
        }

        public static FFmpegVideoModel GetVideoParams(string inPath)
        {
            FFmpegVideoModel model = new FFmpegVideoModel();
            using (VideoFileReader reader = new VideoFileReader())
            {
                reader.Open(inPath);
                model.TotalVideoFrames = reader.FrameCount;
                model.Width = reader.Width;
                model.Height = reader.Height;
                reader.Close();
            }
            return model;
        }
    }
}
