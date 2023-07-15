using SmallHax.SimpleLexicon.Data;
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
    public class EditcParser
    {
        private const string PriorityTagName = "P";
        private const string RowParseRule = "^(?<Kanji>.*?)\\s(?:\\[(?<Kana>.*?)\\]\\s)?(?:\\/(?<Gloss>.*?))?(?:\\/\\((?<PriorityFlag>P)\\))?\\/$";
        private const string TagFindingRule = "\\((?<Tags>[^() ]+)\\)";

        public async Task<Lexicon> Parse(Stream stream, string encodingName)
        {
            var lexicon = new Lexicon();
            var definitions = new Dictionary<string, Definition>();
            var encoding = CodePagesEncodingProvider.Instance.GetEncoding(encodingName);
            var streamReader = new StreamReader(stream, encoding);
            while (!streamReader.EndOfStream)
            {
                var line = await streamReader.ReadLineAsync();

                var rowMatch = Regex.Match(line, RowParseRule);
                if (!rowMatch.Success)
                {
                    throw new Exception($"Unable to parse row {line}");
                }

                var gloss = rowMatch.Groups["Gloss"].Value.Trim();

                definitions.TryGetValue(gloss, out var definition);
                if (definition == null)
                {
                    var tagMatchs = Regex.Matches(gloss, TagFindingRule);
                    var tags = tagMatchs.Cast<Match>().Select(x => x.Groups["Tags"].Value.Split(',')).SelectMany(x => x).Select(x => x.Trim()).Distinct().ToList();
                    definition = new Definition
                    {
                        Text = gloss,
                        Tags = tags,
                    };

                    definitions[gloss] = definition;
                }

                var word = new Word
                {
                    Value = rowMatch.Groups["Kanji"].Value.Trim(),
                    Reading = rowMatch.Groups["Kana"]?.Value.Trim(),
                    Definition = definition,
                    Tags = new List<string>()
                };

                lexicon.Index[word.Value] = word;
                if (!string.IsNullOrEmpty(word.Reading))
                {
                    lexicon.Index[word.Reading] = word;
                };

                var hasPriorityFlag = rowMatch.Groups["PriorityFlag"].Value == PriorityTagName;
                if (hasPriorityFlag)
                {
                    word.Tags.Add(PriorityTagName);
                }
            }
            return lexicon;
        }

    }
}
