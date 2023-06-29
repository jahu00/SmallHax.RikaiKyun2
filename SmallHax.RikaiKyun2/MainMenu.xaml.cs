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
        //await Shell.Current.GoToAsync("//Reader");
        //Spinner.IsRunning = true;
        Spinner.IsVisible = true;
    }
}