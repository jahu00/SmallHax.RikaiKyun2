using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmallHax.Lexicon.Importers
{
    public class EdictImporter
    {
        private readonly Data.LexiconContext _context;
        private const string LexiconTypeName = "EDICT";
        private const string LexiconName = "EDICT";
        private const string PriorityTagName = "P";
        private const string RowParseRule = "^(?<Kanji>.*?)\\s(?:\\[(?<Kana>.*?)\\]\\s)?(?:\\/(?<Gloss>.*?))?(?:\\/\\((?<PriorityFlag>P)\\))?\\/$";
        private const string TagFindingRule = "\\((?<Tags>[^() ]+)\\)";

        public EdictImporter(Data.LexiconContext context)
        {
            _context = context;
        }

        public async Task Import(Stream stream, string encodingName)
        {
            var lexiconType = _context.LexiconTypes.SingleOrDefault(x => x.Name == LexiconTypeName);
            if (lexiconType == null)
            {
                lexiconType = new Models.LexiconType
                {
                    Name = LexiconTypeName,
                    Description = ""
                };
                _context.LexiconTypes.Add(lexiconType);
                _context.SaveChanges();
            }
            var lexicon = _context.Lexicons.SingleOrDefault(x => x.Name == LexiconName);
            if (lexicon == null)
            {
                lexicon = new Models.Lexicon
                {
                    Name = LexiconName,
                    Description = "",
                    LexiconType = lexiconType
                };
                _context.Lexicons.Add(lexicon);
                _context.SaveChanges();
            }
            var priorityTag = _context.Tags.SingleOrDefault(x => x.Name == PriorityTagName);
            if (priorityTag == null)
            {
                priorityTag = new Models.Tag
                {
                    Name = PriorityTagName,
                    Description = "Priority",
                    LexiconType = lexiconType

                };
                _context.Tags.Add(priorityTag);
                _context.SaveChanges();
            }
            var transaction = _context.Database.BeginTransaction();
            var tags = _context.Tags.Where(x => x.LexiconTypeId == lexiconType.Id).ToDictionary(x => x.Name, x => x);
            var definitions = _context.Definitions.Where(x => x.LexiconId == lexicon.Id).ToDictionary(x => x.Text, x => x);
            var entries = _context.Entries.Where(x => x.LexiconId == lexicon.Id).ToDictionary(x => (x.Word, x.Definition), x => x);
            var encoding = CodePagesEncodingProvider.Instance.GetEncoding(encodingName);
            var streamReader = new StreamReader(stream);
            while (!streamReader.EndOfStream)
            {
                var line = await streamReader.ReadLineAsync();
                ProcessLine(line, lexiconType, lexicon, priorityTag, tags, definitions, entries);
            }
            //_context.Tags.AddRange(tags.Values);
            //_context.Definitions.AddRange(definitions.Values);
            //_context.Entries.AddRange(entries.Values);
            _context.SaveChanges();
            transaction.Commit();
        }

        public void ProcessLine(string row, Models.LexiconType lexiconType, Models.Lexicon lexicon, Models.Tag priorityTag, Dictionary<string, Models.Tag> tags, Dictionary<string, Models.Definition> definitions, Dictionary<(string, Models.Definition), Models.Entry> entries)
        {
            var rowMatch = Regex.Match(row, RowParseRule);
            if (!rowMatch.Success)
            {
                throw new Exception($"Unable to parse row {row}");
            }
            var kanji = rowMatch.Groups["Kanji"].Value.Trim();
            var kana = rowMatch.Groups["Kana"].Value.Trim();
            var gloss = rowMatch.Groups["Gloss"].Value.Trim();
            var hasPriorityFlag = rowMatch.Groups["PriorityFlag"].Value == PriorityTagName;

            definitions.TryGetValue(gloss, out var definition);
            if (definition == null)
            {
                definition = new Models.Definition
                {
                    Text = gloss,
                    Lexicon = lexicon,
                    DefinitionTags = new List<Models.DefinitionTag>()
                };

                definitions[gloss] = definition;
                _context.Definitions.Add(definition);

                var tagMatchs = Regex.Matches(gloss, TagFindingRule);
                foreach (Match tagMatch in tagMatchs)
                {
                    var tagValues = tagMatch.Groups["Tags"].Value.Split(',').Select(x => x.Trim());
                    foreach (var tagValue in tagValues)
                    {
                        tags.TryGetValue(tagValue, out var tag);
                        if (tag == null)
                        {
                            tag = new Models.Tag
                            {
                                Name = tagValue,
                                LexiconType = lexiconType
                            };
                            tags[tag.Name] = tag;
                        }
                        if (definition.DefinitionTags.Any(x => x.Tag == tag))
                        {
                            continue;
                        }
                        definition.DefinitionTags.Add(new Models.DefinitionTag { Tag = tag });
                    }
                }
            }

            var entryKey = (kanji, definition);

            entries.TryGetValue(entryKey, out var entry);
            if (entry == null)
            {
                entry = new Models.Entry
                {
                    Lexicon = lexicon,
                    Word = kanji,
                    Definition = definition,
                };

                entries[entryKey] = entry;
                _context.Entries.Add(entry);
            }

            if (string.IsNullOrWhiteSpace(kana))
            {
                if (hasPriorityFlag)
                {
                    entry.EntryTags = new List<Models.EntryTag> { new Models.EntryTag { Tag = priorityTag } };
                }
                return;
            }

            if (entry.Readings == null)
            {
                entry.Readings = new List<Models.Reading>();
            }

            var reading = new Models.Reading
            {
                Text = kana,
            };
            entry.Readings.Add(reading);

            if (hasPriorityFlag == true)
            {
                reading.ReadingTags = new List<Models.ReadingTag> { new Models.ReadingTag { Tag = priorityTag } };
            }


        }
    }
}
