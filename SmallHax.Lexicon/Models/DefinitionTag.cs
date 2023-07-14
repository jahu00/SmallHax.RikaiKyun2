using System;
using System.Collections.Generic;
using System.Text;

namespace SmallHax.Lexicon.Models
{
    public class DefinitionTag
    {
        public int DefinitionId { get; set; }
        public Definition Definition { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
