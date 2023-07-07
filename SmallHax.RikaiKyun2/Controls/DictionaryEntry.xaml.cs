namespace SmallHax.RikaiKyun2.Controls;

public partial class DictionaryEntry : ContentView
{
	public DictionaryEntry(string text)
	{
		InitializeComponent();
		Label.Text = text;
	}
}