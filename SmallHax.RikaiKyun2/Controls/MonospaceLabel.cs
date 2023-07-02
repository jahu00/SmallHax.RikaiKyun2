using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SmallHax.RikaiKyun2.Services;
using System.ComponentModel;
using System.Reflection;

namespace SmallHax.RikaiKyun2.Controls
{
    public class MonospaceLabel : SKCanvasView
    {
        public int Cols { get; private set; } = 0;
        public int Rows { get; private set; } = 0;

        private string[] UpdatablePropertyNames = { nameof(Text), nameof(Width), nameof(FontSize), nameof(FontFamily) };
        private FontService _fontService;

        public string Text { get; set; }
        public double FontSize { get; set; }
        public string FontFamily { get; set; }

        public MonospaceLabel() : base()
        {
            //Drawable = new TextRendererDrawable(this);
            PropertyChanged += OnPropertyChanged;
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            _fontService = Handler.MauiContext.Services.GetService<FontService>();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (UpdatablePropertyNames.Contains(e.PropertyName))
            {
                UpdateText();
            }
        }


        public void UpdateText()
        {
            //HeightRequest
            if (Width == 0)
            {
                return;
            }
            Cols = (int)Math.Floor(Width / FontSize);
            Rows = (int)Math.Ceiling(Text.Length / (double)Cols);
            HeightRequest = FontSize * Rows;
            InvalidateSurface();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            if (Handler == null)
            {
                return;
            }
            var canvas = e.Surface.Canvas;
            canvas.Clear();
            //_canvas.DrawRect(0, 0, (float)Width, (float)Height, new SKPaint { Color = SKColors.Blue });
            var col = 0;
            var row = 1;

            Console.WriteLine(DeviceDisplay.MainDisplayInfo.Density);

            var fontSize = (float)FontSize;
            var font = _fontService.GetFont("NotoSansJP-Light.ttf");
            var paint = new SKPaint(font) { Color = SKColors.Black, TextSize = fontSize };
            foreach (var character in Text)
            {
                canvas.DrawText(character.ToString(), new SKPoint(col * fontSize, row * fontSize), paint);
                col++;
                if (col % Cols == 0)
                {
                    col = 0;
                    row++;
                }
            }

        }
    }
}
