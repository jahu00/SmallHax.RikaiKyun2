using System;
using System.Collections.Generic;
using System.Text;

namespace SmallHax.Lexicon.Models
{
    public class ReadingTag
    {
        public int ReadingId { get; set; }
        public Reading Reading { get;set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
