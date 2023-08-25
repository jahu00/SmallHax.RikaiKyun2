using System;
using System.Collections.Generic;
using System.Text;

namespace SmallHax.SimpleLexicon.Data
{
    public class Lookup
    {
        public string Word { get; set; }
        public string DeinflectedWord { get; set; }
        public string[] RequiredTags { get; set; }
        public string[] Inflections { get; set; }
    }
}
