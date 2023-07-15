using System;
using System.Collections.Generic;

namespace SmallHax.SimpleLexicon.Data
{
    public class Lexicon
    {
        public Dictionary<string, Word> Index { get; private set; } = new Dictionary<string, Word>();
    }
}
