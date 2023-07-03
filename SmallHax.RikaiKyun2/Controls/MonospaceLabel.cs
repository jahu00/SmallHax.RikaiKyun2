using Microsoft.UI.Xaml.Controls;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using SmallHax.RikaiKyun2.Models;
using SmallHax.RikaiKyun2.Services;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SmallHax.RikaiKyun2.Controls
{
    public class MonospaceLabel : SKCanvasView
    {
        private string[] UpdatablePropertyNames = { nameof(Text), nameof(Width), nameof(FontSize), nameof(FontFamily), nameof(Parent) };
        private FontService _fontService;

        private string text { get; set; }
        public string Text { get { return text; } set { text = value; base.OnPropertyChanged(); } }
        private List<CharacterData> Characters { get; set; } = new List<CharacterData>();
        public double FontSize { get; set; }
        public string FontFamily { get; set; }

        public bool OutOfBound { get; set; }

        public MonospaceLabel() : base()
        {
            PropertyChanged += OnPropertyChanged;
        }

        protected override void OnHandlerChanged()
        {
            base.OnHandlerChanged();
            _fontService = Handler.MauiContext.Services.GetService<FontService>();
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Text))
            {
                UpdateText();
            }
            if (UpdatablePropertyNames.Contains(e.PropertyName))
            {
                LayoutText();
            }
        }

        public void UpdateText()
        {
            if (Text == null)
            {
                Characters = new List<CharacterData>();
                return;
            }
            Characters = Text.Select((x, i) => new CharacterData { Character = x.ToString(), Index = i }).ToList();
        }

        public void LayoutText()
        {
            if (Width == 0)
            {
                return;
            }
            if (Characters.Count == 0)
            {
                HeightRequest = 0;
                return;
            }
            var fontSize = (float)FontSize;
            var x = 0f;
            var y = 0f;

            foreach(var characterData in Characters)
            {
                characterData.Rect = new SKRect(x, y, x + fontSize, y + fontSize);
                characterData.Origin = new SKPoint(x, characterData.Rect.Bottom);
                x += fontSize;
                if (x >= Width)
                {
                    x = 0f;
                    y += fontSize;
                }
            }
            HeightRequest = Characters.Last().Rect.Bottom;
            InvalidateSurface();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);
            if (Handler == null || Parent == null || Characters.Count == 0)// Parent is not IView)
            {
                return;
            }
            /*if (!((IView)Parent).Frame.Contains(Frame))
            {
                return;
            }*/
            var canvas = e.Surface.Canvas;
            canvas.Clear();

            Console.WriteLine(DeviceDisplay.MainDisplayInfo.Density);

            var fontSize = (float)FontSize;
            var font = _fontService.GetFont("NotoSansJP-Light.ttf");
            var paint = new SKPaint(font) { Color = SKColors.Black, TextSize = fontSize };
            foreach (var characterData in Characters)
            {
                canvas.DrawText(characterData.Character, characterData.Origin, paint);
            }

        }
    }
}
