using System;
using System.Collections.Generic;
using System.Text;

namespace SmallHax.Lexicon.Models
{
    public class EntryTag
    {
        public int EntryId { get; set; }
        public Entry Entry { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
