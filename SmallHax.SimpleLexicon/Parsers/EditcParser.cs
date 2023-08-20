using SmallHax.SimpleLexicon.Data;
using SmallHax.SimpleLexicon.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmallHax.SimpleLexicon.Parsers
{
    public class EdictParser: ParserBase, IParser
    {
        protected const string newLineSequence = "\n";
        protected const string RowParseRule = "^(?<Kanji>.*?)\\s(?:\\[(?<Kana>.*?)\\]\\s)?(?:\\/(?<Gloss>.*?))?\\/$";
        protected const string TagFindingRule = "\\((?<Tags>[^() ]+)\\)";
        protected readonly Regex rowParseRegex;
        protected readonly Regex tagFindingRegex;

        public EdictParser()
        {
            rowParseRegex = new Regex(RowParseRule, RegexOptions.Compiled);
            tagFindingRegex = new Regex(TagFindingRule, RegexOptions.Compiled);
        }

        public Entry ParseLine(string line)
        {
            var rowMatch = rowParseRegex.Match(line);
            if (!rowMatch.Success)
            {
                throw new Exception($"Unable to parse row \"{line}\"");
            }

            var gloss = rowMatch.Groups["Gloss"].Value.Trim();

            var tagMatchs = tagFindingRegex.Matches(gloss);
            var tags = tagMatchs.Cast<Match>().Select(x => x.Groups["Tags"].Value.Split(',')).SelectMany(x => x).Select(x => x.Trim()).Distinct().ToList();

            var word = new Entry
            {
                Spelling = rowMatch.Groups["Kanji"].Value.Trim(),
                Reading = rowMatch.Groups["Kana"]?.Value.Trim(),
                Definition = gloss,
                Tags = tags
            };
            return word;
        }

        public virtual async Task<Dictionary<string, List<uint>>> BuildIndex(Stream stream, Encoding encoding)
        {
            using var streamReader = new StreamReader(stream, encoding: encoding, detectEncodingFromByteOrderMarks: false, bufferSize: -1, leaveOpen: true);
            var index = new Dictionary<string, List<uint>>();
            var preamble = encoding.GetPreamble();
            long position = preamble.Length;
            while (!streamReader.EndOfStream)
            {
                var startPosition = position;
                var line = await streamReader.ReadLineAsync();
                position = streamReader.GetPosition();
                var rowMatch = rowParseRegex.Match(line);
                if (!rowMatch.Success)
                {
                    continue;
                }
                var kanji = rowMatch.Groups["Kanji"].Value.Trim();
                var reading = rowMatch.Groups["Kana"]?.Value.Trim();
                UpdateIndex(kanji, index, startPosition);
                UpdateIndex(reading, index, startPosition);
            }
            return index;
        }

        protected void UpdateIndex(string word, Dictionary<string, List<uint>> index, long position)
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

        
    }
}
