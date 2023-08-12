using SmallHax.SimpleLexicon.Data;
using SmallHax.SimpleLexicon.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.SimpleLexicon.Service
{
    public class DictionaryService : IDictionaryService, IDisposable
    {
        private Dictionary<string, List<uint>> index;
        private readonly Stream dictionaryStream;
        private readonly StreamReader dictionaryStreamReader;
        private IParser parser = new EditcParser();
        private Encoding dictionaryEncoding;
        private Encoding indexEncoding;

        public bool IsIndexReady => index != null;

        public DictionaryService(Stream dictionaryStream, string dictionaryEncodingName, string indexEncodingName = null)
        {
            this.dictionaryStream = dictionaryStream;
            dictionaryEncoding = CodePagesEncodingProvider.Instance.GetEncoding(dictionaryEncodingName);
            if (string.IsNullOrEmpty(indexEncodingName))
            {
                indexEncoding = dictionaryEncoding;
            }
            dictionaryStreamReader = new StreamReader(this.dictionaryStream, encoding: dictionaryEncoding, detectEncodingFromByteOrderMarks: false, bufferSize: -1, leaveOpen: true);
        }

        public async Task LoadIndex(Stream indexStream)
        {
            index = await parser.ParseIndex(indexStream, indexEncoding);
        }

        public async Task BuildIndex()
        {
            index = await parser.BuildIndex(dictionaryStream, dictionaryEncoding);
        }

        protected async Task<List<string>> Lookup(string word)
        {
            var result = new List<string>();
            if (index.TryGetValue(word, out var positions))
            {
                foreach (var position in positions)
                {
                    // Buffer needs to be discarded if I want StreamReader to read from stream's position.
                    dictionaryStreamReader.DiscardBufferedData();
                    dictionaryStream.Position = position;
                    var row = await dictionaryStreamReader.ReadLineAsync();
                    result.Add(row);
                }
            }
            return result;
        }

        public async Task<List<SearchResult>> Search(List<string> lookups)
        {
            if (index == null)
            {
                await BuildIndex();
            }
            var result = new List<SearchResult>();
            foreach(var lookup in lookups)
            {
                var lookupResults = await Lookup(lookup);
                var searchResults = lookupResults.Select(line => {
                    try
                    {
                        var searchResult = parser.ParseLine(line);
                        return new SearchResult { Lookup = lookup, Result = searchResult };
                    }
                    catch(Exception e)
                    {
                        throw new Exception($"Error preparing search result for lookup \"{lookup}\" from line \"{line}\"");
                    }
                });
                result.AddRange(searchResults);
            }
            return result;
        }

        public void Dispose()
        {
            dictionaryStreamReader?.Dispose();
            dictionaryStream?.Dispose();
        }
    }
}
