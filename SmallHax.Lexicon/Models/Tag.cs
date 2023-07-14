using System;
using System.Collections.Generic;
using System.Text;

namespace SmallHax.Lexicon.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public int LexiconTypeId { get; set; }
        public LexiconType LexiconType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ReadingTag> ReadingTags { get; set; }
        public List<DefinitionTag> DefinitionTags { get; set; }
        public List<DeinflectTag> DeinflectTags { get; set; }
        public List<EntryTag> EntryTags { get; internal set; }
    }
}
