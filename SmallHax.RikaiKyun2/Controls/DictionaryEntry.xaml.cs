using SmallHax.SimpleLexicon.Data;

namespace SmallHax.RikaiKyun2.Controls;

public partial class DictionaryEntry : ContentView
{
	public DictionaryEntry(SearchResult searchResult)
	{
		InitializeComponent();
		var text = searchResult.Result.Spelling;
		if (!string.IsNullOrWhiteSpace(searchResult.Result.Reading))
		{
			text += $" [{searchResult.Result.Reading}]";
		}
		text += $" {searchResult.Result.Definition}";
		Label.Text = text;
	}
}