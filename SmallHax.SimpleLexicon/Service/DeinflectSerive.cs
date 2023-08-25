using SmallHax.SimpleLexicon.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SmallHax.SimpleLexicon.Service
{
    public class DeinflectSerive
    {
        private string[] ValidTags { get; set; }
        private Dictionary<string, Deinflect> Deinflects { get; set; }
        private List<DeinflectRule> DeinflectRules { get; set; }

        public DeinflectSerive(Stream stream)
        {
            var reader = new StreamReader(stream);
            ValidTags = ReadValidTags(reader);
            Deinflects = ReadDeinflects(reader);
            DeinflectRules = ReadDeinflectRules(reader);
        }

        private string[] ReadValidTags(StreamReader reader)
        {
            var line = reader.ReadLine();
            return line.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(s => s.Trim()).ToArray();
        }

        public List<Lookup> Deinflect(string startingWord, string word = null, string[] inflections = null, string[] tags = null, int watchdog = 255)
        {
            watchdog--;
            if (watchdog == 0)
            {
                throw new Exception("Watchdog triggered");
            }
            var result = new List<Lookup>();
            if (inflections == null)
            {
                inflections = new string[0];
            }
            if (tags == null)
            {
                tags = new string[0];
            }
            if (word == null)
            {
                word = startingWord;
            }
            var lookup = new Lookup
            {
                DeinflectedWord = word,
                Inflections = inflections,
                RequiredTags = tags,
                Word = startingWord,
            };
            result.Add(lookup);
            foreach (var rule in DeinflectRules)
            {
                var deinflect = Deinflects[rule.DeinflectId];
                if (tags.Any() && !tags.Contains(deinflect.Tag))
                {
                    continue;
                }
                string deinflectedWord;
                if (rule.Type == DeinflectType.Suffix)
                {
                    if (!word.EndsWith(rule.Text))
                    {
                        continue;
                    }
                    deinflectedWord = word.Substring(0, word.Length - rule.Text.Length) + rule.Replace;
                }
                else if (rule.Type == DeinflectType.Prefix)
                {
                    if (!word.StartsWith(rule.Text))
                    {
                        continue;
                    }
                    deinflectedWord = word.Substring(0, word.Length - rule.Text.Length) + rule.Replace;
                }
                else
                {
                    throw new Exception($"Unsupported deinflect type {rule.Type}");
                }
                var derivativeInflections = new[] { deinflect.Name }.Concat(inflections).ToArray();
                var derivatives = Deinflect
                (
                    startingWord: startingWord,
                    word: deinflectedWord,
                    inflections: derivativeInflections,
                    tags: rule.Tags,
                    watchdog: watchdog
                );
                result.AddRange(derivatives);
            }
            return result;
        }

        public List<Lookup> CleanLookups(List<Lookup> lookups)
        {
            return lookups.Where(lookup => !lookup.RequiredTags.Any() || ValidTags.Any(validTag => lookup.RequiredTags.Contains(validTag))).ToList();
        }

        private List<DeinflectRule> ReadDeinflectRules(StreamReader reader)
        {
            var result = new List<DeinflectRule>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }
                var split = line.Split('/');
                var deinflect = new DeinflectRule
                {
                    DeinflectId = split[0],
                    Type = split[1] == "S" ? DeinflectType.Suffix : DeinflectType.Prefix,
                    Text = split[2],
                    Replace = split[3],
                    Tags = split[4].Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()).ToArray()
                };
                result.Add(deinflect);
            }
            return result;
        }

        private Dictionary<string,Deinflect> ReadDeinflects(StreamReader reader)
        {
            var result = new List<Deinflect>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                {
                    break;
                }
                var split = line.Split('/');
                var deinflect = new Deinflect
                {
                    Id = split[0],
                    Name = split[2],
                    Tag = split[1],
                };
                result.Add(deinflect);
            }
            return result.ToDictionary(x => x.Id, x => x);
        }

    }
}
