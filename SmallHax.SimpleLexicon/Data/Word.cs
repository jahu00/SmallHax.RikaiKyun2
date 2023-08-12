using System;
using System.Collections.Generic;
using System.Text;

namespace SmallHax.SimpleLexicon.Data
{
    public class Entry
    {
        public string Spelling { get; set; }
        public string Reading { get; set; }
        public string Definition { get; set; }
        public List<string> Tags { get; set; }
    }
}
