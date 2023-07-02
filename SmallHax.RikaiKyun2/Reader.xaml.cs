using Microsoft.Maui.Graphics;
using SmallHax.RikaiKyun2.Controls;
using SmallHax.RikaiKyun2.Services;

namespace SmallHax.RikaiKyun2;

public partial class Reader : ContentPage
{
    private DocumentService _documentService;

    public Reader()
	{
		InitializeComponent();
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
        Container.Clear();
        var i = 0;
        foreach (var node in _documentService.Document.Children)
        {
            if (i == 10)
            {
                return;
            }
            var label = new MonospaceLabel { Text = node.Value, FontSize = 52, IgnorePixelScaling = true, VerticalOptions = LayoutOptions.FillAndExpand, HorizontalOptions = LayoutOptions.FillAndExpand };
            Container.Children.Add(label);
            i++;
        }
    }
}