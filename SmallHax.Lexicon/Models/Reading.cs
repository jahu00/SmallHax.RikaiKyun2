using System;
using System.Collections.Generic;
using System.Text;

namespace SmallHax.Lexicon.Models
{
    public class Reading
    {
        public int Id { get; set; }
        public int EntryId { get; set; }
        public Entry Entry { get; set; }
        public string Text { get; set; }
        public List<ReadingTag> ReadingTags { get; set; }
    }
}
