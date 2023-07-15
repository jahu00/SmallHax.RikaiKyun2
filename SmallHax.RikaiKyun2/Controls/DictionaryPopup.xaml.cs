using SmallHax.SimpleLexicon.Data;
using SmallHax.SimpleLexicon.Parsers;
using System.Reflection;
using System.Xml.Linq;

namespace SmallHax.RikaiKyun2.Controls;

public partial class DictionaryPopup : ContentView
{
    private Lexicon _lexicon;

    public DictionaryPopup()
	{
		InitializeComponent();
	}

	public async Task LoadLexicon()
	{
        var parser = new EditcParser();
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.Dictionaries.Japanese.edict");
        _lexicon = await parser.Parse(stream, "euc-jp");
    }

	public async Task<List<SearchResult>> Search(List<string> lookups)
	{
		if (_lexicon == null)
		{
			await LoadLexicon();
		}
		var results = new List<SearchResult>();
		foreach (var lookup in lookups.OrderByDescending(x => x.Length))
		{
			_lexicon.Index.TryGetValue(lookup, out var word);
			if (word == null)
			{
				continue;
			}
			var result = new SearchResult { Lookup = lookup, Result = word };
			results.Add(result);
		}
		return results;
	}

	public void Populate(List<SearchResult> searchResults)
	{
		EntriesContainer.Clear();
		foreach (var searchResult in searchResults)
		{
			var entryControl = new DictionaryEntry(searchResult);
			EntriesContainer.Add(entryControl);
		}
	}
}