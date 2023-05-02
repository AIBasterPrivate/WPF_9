using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_9.video
{
    public interface IVideoEmbending
    {
        long TotalVideoFrames { get; set; }
        int Width { get; set; }
        int Height { get; set; }

    }
}
