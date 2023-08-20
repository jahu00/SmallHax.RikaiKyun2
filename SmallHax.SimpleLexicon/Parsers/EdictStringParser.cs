using SmallHax.SimpleLexicon.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.SimpleLexicon.Parsers
{
    public class EdictStringParser: EdictParser
    {
        public override Task<Dictionary<string, List<uint>>> BuildIndex(Stream stream, Encoding encoding)
        {
            using var streamReader = new StreamReader(stream, encoding: encoding, detectEncodingFromByteOrderMarks: false, bufferSize: -1, leaveOpen: true);
            var index = new Dictionary<string, List<uint>>();
            var dictionaryString = streamReader.ReadToEnd();
            int position = 0;
            int startPosition;
            int endPosition;

            do
            {
                startPosition = position;
                position = dictionaryString.IndexOf(newLineSequence, position);
                if (position == -1)
                {
                    endPosition = dictionaryString.Length;
                }
                else
                {
                    endPosition = position;
                    position += newLineSequence.Length;
                }
                var line = dictionaryString.Substring(startPosition, endPosition - startPosition);
                var rowMatch = rowParseRegex.Match(line);
                if (!rowMatch.Success)
                {
                    continue;
                }
                var kanji = rowMatch.Groups["Kanji"].Value.Trim();
                var reading = rowMatch.Groups["Kana"]?.Value.Trim();
                UpdateIndex(kanji, index, startPosition);
                UpdateIndex(reading, index, startPosition);
            } while (position > -1);

            return Task.FromResult(index);
        }
    }
}
