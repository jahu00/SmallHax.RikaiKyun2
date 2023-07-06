using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.RikaiKyun2.Models
{
    public class Document
    {
        public int Length { get; private set; }
        public List<Node> Nodes { get; private set; } = new List<Node>();
        public static async Task<Document> FromTxt(string fileName)
        {
            var result = new Document();
            var text = await File.ReadAllTextAsync(fileName);
            var lines = text.Split(new char[] { '\n', '\r' }).Where(x => x.Trim() != string.Empty);
            var i = 0;
            foreach (var line in lines)
            {
                var child = new Node() { Id = i, Text = line, Position = result.Length };
                result.Nodes.Add(child);
                result.Length += child.Text.Length;
                i++;
            }
            return result;
        }
    }
}
