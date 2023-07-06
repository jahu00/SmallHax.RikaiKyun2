using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SmallHax.RikaiKyun2.Models;
using SmallHax.RikaiKyun2.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SmallHax.RikaiKyun2.Controls
{
    public class DocumentRenderer : SKCanvasView
    {
        private StyleService _styleService;
        private Document _document;
        public Document Document { get { return _document; } set { _document = value; base.OnPropertyChanged(); } }
        public List<TextLayout> Layouts { get; private set; } = new List<TextLayout>();
        public int NodeId { get; private set; }
        public float Offset { get; private set; }
        public double Progress { get; private set; }

        private int? selectedNodeId;
        private int? selectStart;
        private int? selectEnd;

        public DocumentRenderer()
        {
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Width))
            {
                AdjustWidth();
            }
            if (e.PropertyName ==  nameof(Width) || e.PropertyName == nameof(Height) || e.PropertyName == nameof(Document))
            {
                UpdateLayouts(true);
                InvalidateSurface();
            }
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            _styleService = Handler.MauiContext.Services.GetService<StyleService>();
        }

        private void AdjustWidth()
        {
            if (Width <= 0)
            {
                return;
            }
            var width = (float)Width;
            var lastY = 0f;
            Layouts.ForEach(layout => {
                layout.Width = width;
                layout.Y = lastY;
                lastY = layout.Bottom;
            });
        }

        private void UpdateLayouts(bool clearInvisible)
        {
            if (Width <= 0 || Height <= 0)
            {
                return;
            }
            if (Document == null || Document.Nodes.Count == 0)
            {
                Layouts.Clear();
                return;
            }
            var width = (float)Width;
            var height = (float)Height;
            var addWatchdog = 1024;
            var firstNode = Document.Nodes.First();
            var lastNode = Document.Nodes.Last();
            var lastLayout = Layouts.LastOrDefault();
            while (Layouts.Count == 0 || lastLayout.Bottom + Offset < height)
            {
                addWatchdog--;
                if (addWatchdog <= 0)
                {
                    break;
                }
                if (lastLayout?.Node.Id == lastNode.Id)
                {
                    break;
                }
                var node = firstNode;
                if (lastLayout != null)
                {
                    node = Document.Nodes[lastLayout.Node.Id + 1];
                }
                var textProperties = _styleService.GetTextProperties(node.Style);
                var layout = new TextLayout(node, width, textProperties);
                if (lastLayout != null)
                {
                    layout.Y = lastLayout.Bottom;
                }
                Layouts.Add(layout);
                lastLayout = layout;
            }

            if (lastLayout.Node.Id == lastNode.Id && lastLayout.Bottom + Offset < height)
            {
                Offset += height - lastLayout.Bottom - Offset;
            }
            if (!clearInvisible)
            {
                return;
            }
            var newLayouts = Layouts.Where(x => x.Bottom + Offset > 0 && x.Y + Offset < Height).ToList();
            if (newLayouts.Count == Layouts.Count)
            {
                return;
            }
            Layouts = newLayouts;
            var firstLayout = Layouts.First();
            Offset += firstLayout.Y;
            Layouts.ForEach(x => { x.Y -= firstLayout.Y; });
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            if (Handler == null || Parent == null || Layouts.Count == 0)
            {
                return;
            }
            
            var canvas = e.Surface.Canvas;
            canvas.Clear();

            Console.WriteLine(DeviceDisplay.MainDisplayInfo.Density);

            foreach (var layout in Layouts)
            {
                canvas.SetMatrix(SKMatrix.CreateTranslation(0, layout.Y + Offset));
                RenderLayout(canvas, layout);
            }

        }

        private void RenderLayout(SKCanvas canvas, TextLayout textLayout)
        {
            var selectPaint = new SKPaint() { Color = SKColors.Blue };
            SKPaint paint;
            foreach (var characterData in textLayout.Characters)
            {
                var formatting = textLayout.Node.GetFormatting(characterData.Index);
                paint = _styleService.GetPaint(textLayout.Node.Style, formatting);
                if (textLayout.Node.Id == selectedNodeId && characterData.Index >= selectStart && characterData.Index <= selectEnd)
                {
                    canvas.DrawRect(characterData.Rect, selectPaint);
                }
                canvas.DrawText(characterData.Character, characterData.Origin, paint);
            }
        }
    }
}
