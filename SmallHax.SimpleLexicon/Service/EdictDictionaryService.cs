using SmallHax.SimpleLexicon.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmallHax.SimpleLexicon.Service
{
    public class EdictDictionaryService : IDictionaryService
    {
        private const string indexSeparator = "/";
        private const string newLineCharacter = "\n";
        private const string PriorityTagName = "P";
        private const string RowParseRule = "^(?<Kanji>.*?)\\s(?:\\[(?<Kana>.*?)\\]\\s)?(?:\\/(?<Gloss>.*?))?(?:\\/\\((?<PriorityFlag>P)\\))?\\/$";
        private const string TagFindingRule = "\\((?<Tags>[^() ]+)\\)";

        public Dictionary<string, List<uint>> Index { get; set; }

        private readonly Regex rowParseRegex;
        private readonly Regex tagFindingRegex;

        public EdictDictionaryService()
        {
            rowParseRegex = new Regex(RowParseRule, RegexOptions.Compiled);
            tagFindingRegex = new Regex(TagFindingRule, RegexOptions.Compiled);
        }

        public async Task<Dictionary<string, List<uint>>> ParseIndex(string indexFileName, string encodingName = "UTF8")
        {
            var encoding = CodePagesEncodingProvider.Instance.GetEncoding(encodingName);
            var stream = File.OpenRead(indexFileName);
            var streamReader = new StreamReader(stream, encoding);
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

        public async Task<Dictionary<string, List<uint>>> BuildIndex(string fileName, string encodingName = "UTF8")
        {
            var encoding = CodePagesEncodingProvider.Instance.GetEncoding(encodingName);
            using var stream = File.OpenRead(fileName);
            var streamReader = new StreamReader(stream, encoding);
            var index = new Dictionary<string, List<uint>>();
            while (!streamReader.EndOfStream)
            {
                var position = stream.Position;
                var line = await streamReader.ReadLineAsync();
                var rowMatch = rowParseRegex.Match(line);
                var kanji = rowMatch.Groups["Kanji"].Value.Trim();
                var reading = rowMatch.Groups["Kana"]?.Value.Trim();
                UpdateIndex(kanji, index, position);
                UpdateIndex(reading, index, position);
            }
            return index;
        }

        private void UpdateIndex(string word, Dictionary<string, List<uint>> index, long position)
        {
            if (string.IsNullOrWhiteSpace(word))
            {
                return;
            }
            if (!index.TryGetValue(word, out var positions))
            {
                positions = new List<uint>();
                index[word] = positions;
            }
            positions.Add((uint)position);
        }

        public async Task SaveIndex(Dictionary<string, List<uint>> index, string fileName, string encodingName = "UTF8")
        {
            var encoding = CodePagesEncodingProvider.Instance.GetEncoding(encodingName);
            using var stream = File.OpenWrite(fileName);
            var streamWriter = new StreamWriter(stream, encoding);
            streamWriter.NewLine = newLineCharacter;
            foreach (var pair in index)
            {
                await streamWriter.WriteLineAsync($"{pair.Key}{indexSeparator}{string.Join(indexSeparator, pair.Value)}");
            }
            streamWriter.Close();
        }

        public Task<IEnumerable<SearchResult>> Search(List<string> lookups)
        {
            throw new NotImplementedException();
        }
    }
}
