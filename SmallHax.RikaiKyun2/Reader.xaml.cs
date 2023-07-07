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

    private void OnTextTapped(DocumentRenderer label, TextTappedEventArgs e)
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
        label.Select(e.Node.Id, e.Character.Index, e.Character.Index + Math.Min(maxLength, 12));
    }
}