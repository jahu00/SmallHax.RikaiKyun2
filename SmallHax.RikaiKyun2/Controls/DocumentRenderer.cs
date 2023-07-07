using Microsoft.Maui.Controls;
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
        public float OldOffset { get; private set; }
        public double Progress { get; private set; }

        private int? selectedNodeId;
        private int? selectStart;
        private int? selectEnd;
        private readonly TapGestureRecognizer tapGestureRecognizer;
        private readonly PanGestureRecognizer panGestureRecognizer;

        public DocumentRenderer()
        {
            PropertyChanged += OnPropertyChanged;

            tapGestureRecognizer = new TapGestureRecognizer { Buttons = ButtonsMask.Primary };
            tapGestureRecognizer.Tapped += OnTapped;
            GestureRecognizers.Add(tapGestureRecognizer);

            panGestureRecognizer = new PanGestureRecognizer();
            panGestureRecognizer.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(panGestureRecognizer);
        }

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            if (e.StatusType == GestureStatus.Completed)
            {
                UpdateLayouts(true);
                InvalidateSurface();
                return;
            }
            if (e.StatusType == GestureStatus.Started)
            {
                OldOffset = Offset;
            }
            Offset = OldOffset + (float)e.TotalY;
            UpdateLayouts(false);
            InvalidateSurface();
        }

        private void OnTapped(object sender, TappedEventArgs e)
        {
            //throw new NotImplementedException();
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
            var firstNode = Document.Nodes.First();
            var lastNode = Document.Nodes.Last();
            var firstLayout = Layouts.FirstOrDefault();
            var lastLayout = Layouts.LastOrDefault();
            var previousWatchdog = 1024;
            while (Layouts.Count > 0 && firstLayout.Y + Offset > 0)
            {
                previousWatchdog--;
                if (previousWatchdog <= 0)
                {
                    break;
                }
                if (firstLayout.Node.Id == firstNode.Id)
                {
                    break;
                }
                var node = Document.Nodes[firstLayout.Node.Id - 1];
                var textProperties = _styleService.GetTextProperties(node.Style);
                var layout = new TextLayout(node, width, textProperties);
                layout.Y = firstLayout.Y - layout.Height;
                Layouts.Add(layout);
                firstLayout = layout;
            }
            var nextWatchdog = 1024;
            while (Layouts.Count == 0 || lastLayout.Bottom + Offset < height)
            {
                nextWatchdog--;
                if (nextWatchdog <= 0)
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
            firstLayout = Layouts.First();
            if (firstLayout.Node.Id == firstNode.Id && firstLayout.Y + Offset > 0)
            {
                Offset = 0;
            }
            else if (lastLayout.Node.Id == lastNode.Id && lastLayout.Bottom + Offset < height)
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
            firstLayout = Layouts.First();
            var firstLayoutY = firstLayout.Y;
            Offset += firstLayoutY;
            Layouts.ForEach(x => { x.Y -= firstLayoutY; });
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
