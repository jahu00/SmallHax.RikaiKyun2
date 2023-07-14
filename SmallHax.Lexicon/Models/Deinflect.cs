using System;
using System.Collections.Generic;
using System.Text;

namespace SmallHax.Lexicon.Models
{
    public class Deinflect
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Rule { get; set; }
        public string Replace { get; set; }
        public List<DeinflectTag> DeinflectTags { get; set; }
    }
}
