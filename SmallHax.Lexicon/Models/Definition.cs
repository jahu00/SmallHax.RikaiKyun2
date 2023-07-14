using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.Lexicon.Models
{
    public class Definition
    {
        public int Id { get; set; }
        public int LexiconId { get; set; }
        public Lexicon Lexicon { get; set; }
        public string Text { get; set; }
        public List<Reading> Readings { get; set; }
        public List<DefinitionTag> DefinitionTags { get; set; }
        public List<Entry> Entries { get; set; }
    }
}
