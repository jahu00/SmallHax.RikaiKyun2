using Microsoft.Maui.Graphics;
using SmallHax.RikaiKyun2.Controls;
using SmallHax.RikaiKyun2.Services;

namespace SmallHax.RikaiKyun2;

public partial class Reader : ContentPage
{
    private DocumentService _documentService;

    private Dictionary<int, MonospaceLabel> nodeIndex;

    public Reader()
	{
		InitializeComponent();
        Renderer.TextTapped += OnTextTapped;
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        _documentService = Handler.MauiContext.Services.GetService<DocumentService>();
        _documentService.DocumentLoaded += OnDocumentLoaded;
        if (_documentService.Document != null)
        {
            OnDocumentLoaded();
        }
    }

    private void OnDocumentLoaded()
    {
        Renderer.Document = _documentService.Document;
    }

    private async void OnTextTapped(DocumentRenderer label, TextTappedEventArgs e)
    {
        if (Renderer.SelectedNodeId.HasValue)
        {
            Renderer.Deselect();
        }
        if (e.Character == null)
        {
            return;
        }
        var maxLength = e.Node.Text.Length - e.Character.Index;
        var length = Math.Min(maxLength, 12);
        var endIndex = e.Character.Index + length;
        var text = e.Node.Text.Substring(e.Character.Index, length);
        var lookups = text.Select((s, i) => text.Substring(0, text.Length - i)).ToList();
        var results = await DictionaryPopup.Search(lookups);
        if (results.Count > 0)
        {
            endIndex = e.Character.Index + results[0].Lookup.Length;
        }
        else
        {
            endIndex = e.Character.Index + 1;
        }
        label.Select(e.Node.Id, e.Character.Index, endIndex);
        DictionaryPopup.Populate(results);
    }

    protected override bool OnBackButtonPressed()
    {
        //return base.OnBackButtonPressed();
        Shell.Current.GoToAsync("//MainMenu");
        return true;
    }
}