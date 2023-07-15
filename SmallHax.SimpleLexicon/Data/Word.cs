using System;
using System.Collections.Generic;
using System.Text;

namespace SmallHax.SimpleLexicon.Data
{
    public class Word
    {
        public string Value { get; set; }
        public string Reading { get; set; }
        public List<string> Tags { get; set; }
        public Definition Definition { get; set; }
    }
}
