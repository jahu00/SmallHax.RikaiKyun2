using System;
using System.Collections.Generic;

namespace SmallHax.SimpleLexicon.Data
{
    public class Lexicon
    {
        public Dictionary<string, List<Word>> Index { get; private set; } = new Dictionary<string, List<Word>>();
    }
}
