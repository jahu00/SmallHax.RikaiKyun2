using Microsoft.Maui.Graphics;
using SmallHax.RikaiKyun2.Controls;
using SmallHax.RikaiKyun2.Services;

namespace SmallHax.RikaiKyun2;

public partial class Reader : ContentPage
{
    private DocumentService _documentService;

    private int? selectedNodeId;

    private Dictionary<int, MonospaceLabel> nodeIndex;

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
        nodeIndex = new Dictionary<int, MonospaceLabel>();
        Container.Clear();
        var i = 0;
        foreach (var node in _documentService.Document.Children)
        {
            if (i == 10)
            {
                //return;
            }
            var label = new MonospaceLabel { Text = node.Value, FontSize = 52, HorizontalOptions = LayoutOptions.FillAndExpand, NodeId = i };
            label.TextTapped += OnTextTapped;
            Container.Children.Add(label);
            nodeIndex[i] = label;
            i++;
        }
    }

    private void OnTextTapped(MonospaceLabel label, int? index)
    {
        if (selectedNodeId.HasValue)
        {
            nodeIndex[selectedNodeId.Value].Deselect();
        }
        if (!index.HasValue)
        {
            return;
        }
        selectedNodeId = label.NodeId;
        var maxLength = label.Text.Length - index.Value;
        label.Select(index.Value, index.Value + Math.Min(maxLength, 12));
    }
}