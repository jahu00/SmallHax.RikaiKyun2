using System;
using System.Collections.Generic;
using System.Text;

namespace SmallHax.Lexicon.Models
{
    public class LexiconType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Lexicon> Lexicons { get; set; }
        public List<Tag> Tags { get; set; }
    }
}
