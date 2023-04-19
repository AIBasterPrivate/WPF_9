﻿using Accord.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPF_9.videos.service;

namespace WPF_9.videos.models
{
    public class FFmpegVideoModel : IVideoEmbending
    {
        public long TotalVideoFrames { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
