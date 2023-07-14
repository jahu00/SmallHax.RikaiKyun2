using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.Lexicon.Models
{
    public class Entry
    {
        public int Id { get; set; }
        public int LexiconId { get; set; }
        public Lexicon Lexicon { get; set; }
        public string Word { get; set; }
        public List<Reading> Readings { get; set; }
        public int DefinitionId { get; set; }
        public Definition Definition { get; set; }
        public List<EntryTag> EntryTags { get; internal set; }
    }
}
