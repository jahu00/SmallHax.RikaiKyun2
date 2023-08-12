using SmallHax.SimpleLexicon.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.SimpleLexicon.Parsers
{
    public interface IParser
    {
        Entry ParseLine(string line);
        Task<Dictionary<string, List<uint>>> ParseIndex(Stream stream, Encoding encoding);
        Task<Dictionary<string, List<uint>>> BuildIndex(Stream stream, Encoding encoding);
        Task SaveIndex(Dictionary<string, List<uint>> index, string fileName, Encoding encoding);
    }
}
