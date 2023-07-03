using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.RikaiKyun2.Models
{
    public class CharacterData
    {
        public string Character { get; set; }
        public SKRect Rect { get; set; }
        public SKPoint Origin { get; set; }
        public int Index { get; set; }
    }
}
