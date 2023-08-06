using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.RikaiKyun2.Models
{
    public class TextNode
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public Style Style { get; set; }
        public string Text { get; set; }
        public Dictionary<int, Formatting> Formatting { get; set; }
        public Dictionary<int, Ruby> Ruby { get; set; }

        public Formatting GetFormatting(int index)
        {
            if (Formatting == null)
            {
                return null;
            }
            Formatting.TryGetValue(index, out var formatting);
            return formatting;
        }
    }
}
