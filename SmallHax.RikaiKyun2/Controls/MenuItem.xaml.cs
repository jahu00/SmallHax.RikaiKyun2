namespace SmallHax.RikaiKyun2.Controls;

public partial class MenuItem : ContentView
{
    private string _text;

    public string Text { get { return _text; } set { SetText(value); } }
    public event Action<object, TappedEventArgs> Tapped;

    private void SetText(string value)
    {
        _text = value;
        Label.Text = _text;
    }

    public MenuItem()
	{
		InitializeComponent();
	}

    void OnTapGestureRecognizerTapped(object sender, TappedEventArgs args)
    {
        Tapped?.Invoke(this, args);
    }
}