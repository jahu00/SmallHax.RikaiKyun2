using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.RikaiKyun2.Models
{
    public class Document: Node
    {
        public int Length { get; private set; }
        public static async Task<Document> FromTxt(string fileName)
        {
            var result = new Document();
            var text = await File.ReadAllTextAsync(fileName);
            result.Children = text.Split(new char[] { '\n', '\r' }).Where(x => x.Trim() != string.Empty).Select(x => new Node() { Value = x }).ToList();
            result.Length = result.Children.Sum(x => x.Value.Length);
            return result;
        }
    }
}
