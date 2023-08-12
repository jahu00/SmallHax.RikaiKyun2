using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.SimpleLexicon.Parsers
{
    public abstract class ParserBase
    {
        public string IndexSeparator { get; set; } = "/";
        public string NewLineCharacter { get; set; } = "\n";

        public virtual async Task<Dictionary<string, List<uint>>> ParseIndex(Stream stream, Encoding encoding)
        {
            using var streamReader = new StreamReader(stream, encoding: encoding, detectEncodingFromByteOrderMarks: false, bufferSize: -1, leaveOpen: true);
            var index = new Dictionary<string, List<uint>>();
            while (!streamReader.EndOfStream)
            {
                var line = await streamReader.ReadLineAsync();
                var split = line.Split('/');
                var word = split[0];
                var adresses = split.Skip(1).Select(x => uint.Parse(x)).ToList();
                index[word] = adresses;
            }
            return index;
        }

        public virtual async Task SaveIndex(Dictionary<string, List<uint>> index, string fileName, Encoding encoding)
        {
            using var stream = File.OpenWrite(fileName);
            using var streamWriter = new StreamWriter(stream, encoding);
            streamWriter.NewLine = NewLineCharacter;
            foreach (var pair in index)
            {
                await streamWriter.WriteLineAsync($"{pair.Key}{IndexSeparator}{string.Join(IndexSeparator, pair.Value)}");
            }
            streamWriter.Close();
        }
    }
}
