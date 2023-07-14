using System;
using System.Collections.Generic;
using System.Text;

namespace SmallHax.Lexicon.Models
{
    public class DeinflectTag
    {
        public int DeinflectId { get; set; }
        public Deinflect Deinflect { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
