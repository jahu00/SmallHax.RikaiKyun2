namespace SmallHax.RikaiKyun2;

public partial class MainMenu : ContentPage
{
	public MainMenu()
	{
		InitializeComponent();
	}

    private void ExitButton_Tapped(object arg1, TappedEventArgs arg2)
    {
        Application.Current.Quit();
    }

    private async void OpenButton_Tapped(object arg1, TappedEventArgs arg2)
    {
        //Application.Current.MainPage = new NavigationPage(new Reader());
        await Shell.Current.GoToAsync("//Reader");
    }
}