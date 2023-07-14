using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.Lexicon.Models
{
    public class Lexicon
    {
        public int Id { get; set; }
        public int LexiconTypeId { get; set; }
        public LexiconType LexiconType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public List<Entry> Entries { get; set; }
        public List<Definition> Definitions { get; set; }
    }
}
