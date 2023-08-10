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
        _lexicon = await parser.ParseAsync(stream, "euc-jp");
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
			_lexicon.Index.TryGetValue(lookup, out var words);
			if (words == null)
			{
				continue;
			}
			var tempResult = words.Select(word => new SearchResult { Lookup = lookup, Result = word });
			results.AddRange(tempResult);
		}
		return results;
	}

	public void Populate(List<SearchResult> searchResults)
	{
		//Label.Text = "ABC";
		//return;
        EntriesContainer.Clear();
        //var htmlEntries = new List<string>();
		foreach (var searchResult in searchResults)
		{
			var entryControl = new DictionaryEntry(searchResult);
			EntriesContainer.Add(entryControl);
			/*var htmlEntry = $"{searchResult.Result.Value} ";
			if (!string.IsNullOrWhiteSpace(searchResult.Result.Reading))
			{
				htmlEntry += $"[{searchResult.Result.Reading}] ";
			}
            htmlEntry += $"- {searchResult.Result.Definition.Text}";
			htmlEntries.Add(htmlEntry);*/
		}
		//Label.Text = string.Join("<br/><br/>", htmlEntries);
	}
}