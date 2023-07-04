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
        private int? _selectStart;
        private int? _selectEnd;
        private readonly TapGestureRecognizer tapGestureRecognizer;

        public int NodeId { get; set; }
        private string text { get; set; }
        public string Text { get { return text; } set { text = value; base.OnPropertyChanged(); } }
        private List<CharacterData> Characters { get; set; } = new List<CharacterData>();
        public double FontSize { get; set; }
        public string FontFamily { get; set; }

        public event Action<MonospaceLabel, int?> TextTapped;

        public MonospaceLabel() : base()
        {
            PropertyChanged += OnPropertyChanged;
            tapGestureRecognizer = new TapGestureRecognizer { Buttons = ButtonsMask.Primary };
            tapGestureRecognizer.Tapped += OnTapped;
            GestureRecognizers.Add(tapGestureRecognizer);
        }

        public void Select(int start, int end)
        {
            _selectStart = start;
            _selectEnd = end;
            InvalidateSurface();
        }

        public void Deselect()
        {
            _selectStart = null;
            _selectEnd = null;
            InvalidateSurface();
        }

        private void OnTapped(object sender, TappedEventArgs e)
        {
            var position = e.GetPosition(this).Value;
            var character = Characters.FirstOrDefault(x => x.Rect.Contains((float)position.X, (float)position.Y));
            TextTapped?.Invoke(this, character?.Index);
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
            var maxWidth = Width - fontSize;

            foreach(var characterData in Characters)
            {
                characterData.Rect = new SKRect(x, y, x + fontSize, y + fontSize);
                characterData.Origin = new SKPoint(x, characterData.Rect.Bottom);
                x += fontSize;
                if (x >= maxWidth)
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
            var selectPaint = new SKPaint(font) { Color = SKColors.Blue };
            foreach (var characterData in Characters)
            {
                if (characterData.Index >= _selectStart && characterData.Index <= _selectEnd)
                {
                    canvas.DrawRect(characterData.Rect, selectPaint);
                }
                canvas.DrawText(characterData.Character, characterData.Origin, paint);
            }

        }
    }
}
