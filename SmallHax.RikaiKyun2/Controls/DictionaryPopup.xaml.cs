namespace SmallHax.RikaiKyun2.Controls;

public partial class DictionaryPopup : ContentView
{
	public DictionaryPopup()
	{
		InitializeComponent();
	}

	public void Populate(string[] entries)
	{
		EntriesContainer.Clear();
		foreach (string entry in entries)
		{
			var entryControl = new DictionaryEntry(entry);
			EntriesContainer.Add(entryControl);
		}
	}
}