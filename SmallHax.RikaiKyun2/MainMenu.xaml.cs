using SmallHax.RikaiKyun2.Services;

namespace SmallHax.RikaiKyun2;

public partial class MainMenu : ContentPage
{
    private DocumentService _documentService;

    public MainMenu()
	{
		InitializeComponent();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        _documentService = Handler.MauiContext.Services.GetService<DocumentService>();
        _documentService.DocumentLoaded += OnDocumentLoaded;
    }

    private void ExitButton_Tapped(object arg1, TappedEventArgs arg2)
    {
        Application.Current.Quit();
    }

    private async void OpenButton_Tapped(object arg1, TappedEventArgs arg2)
    {
        var readPermissionStatus = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
        if (readPermissionStatus != PermissionStatus.Granted)
        {
            await Permissions.RequestAsync<Permissions.StorageRead>();
        }
        var result = await FilePicker.PickAsync(PickOptions.Default);
        if (result == null)
        {
            return;
        }
        Spinner.IsVisible = true;
        await _documentService.Open(result.FullPath);
    }

    private async void OnDocumentLoaded()
    {
        await Shell.Current.GoToAsync("//Reader");
        Spinner.IsVisible = false;
    }
}