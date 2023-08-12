using SmallHax.SimpleLexicon.Data;
using SmallHax.SimpleLexicon.Parsers;
using SmallHax.SimpleLexicon.Service;
using System.ComponentModel.Design;
using System.Reflection;
using System.Xml.Linq;

namespace SmallHax.RikaiKyun2.Controls;

public partial class DictionaryPopup : ContentView
{
    private DictionaryService _dictionaryService;

    public DictionaryPopup()
	{
		InitializeComponent();
	}

	public async Task LoadDictionary()
	{
        var assembly = Assembly.GetExecutingAssembly();
        var dictionaryStream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.Dictionaries.Japanese.edict");
        using var indexStream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.Dictionaries.Japanese.edict_index");
		_dictionaryService = new DictionaryService(dictionaryStream, "euc-jp");
		await _dictionaryService.LoadIndex(indexStream);

    }

	public async Task<List<SearchResult>> Search(List<string> lookups)
	{
		if (_dictionaryService == null)
		{
			await LoadDictionary();
		}
		var results = await _dictionaryService.Search(lookups);
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