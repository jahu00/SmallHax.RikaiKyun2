using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.RikaiKyun2.Models
{
    public class Document: Node
    {
        public static async Task<Document> FromTxt(string fileName)
        {
            var result = new Document();
            var text = await File.ReadAllTextAsync(fileName);
            result.Children = text.Split('\n').Select(x => new Node() { Value = x }).ToList();
            return result;
        }
    }
}
