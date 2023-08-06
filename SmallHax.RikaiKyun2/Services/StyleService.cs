using Microsoft.Maui.Graphics;
using SkiaSharp;
using SmallHax.RikaiKyun2.Models;

namespace SmallHax.RikaiKyun2.Services
{
    public class StyleService
    {
        private Dictionary<string, SKPaint> PaintStore = new Dictionary<string, SKPaint>();
        private Dictionary<string, TextProperties> PropertiesStore = new Dictionary<string, TextProperties>();
        private FontService _fontService;

        public StyleService(FontService fontService)
        {
            _fontService = fontService;
        }

        public SKPaint GetSelectPaint()
        {
            var key = "Select";
            if (PaintStore.TryGetValue(key, out var selectPaint))
            {
                return selectPaint;
            }
            selectPaint = new SKPaint() { Color = SKColors.LightBlue };
            PaintStore[key] = selectPaint;
            return selectPaint;
        }

        public SKPaint GetTextPaint(Models.Style style, Formatting formatting)
        {
            var key = style.ToString();
            if (formatting != null && formatting.Bold)
            {
                key += "_" + nameof(formatting.Bold);
            }
            if (PaintStore.TryGetValue(key, out var paint))
            {
                return paint;
            }
            var fontSuffix = "Light";
            if (formatting != null && formatting.Bold)
            {
                fontSuffix = "Bold";
            }
            switch (style)
            {
                case Models.Style.Paragraph:
                    paint = new SKPaint(_fontService.GetFont($"NotoSansJP-{fontSuffix}.ttf")) { Color = SKColors.Black, TextSize = 20 };
                    break;
                case Models.Style.Header:
                    paint = new SKPaint(_fontService.GetFont($"NotoSerifJP-{fontSuffix}.ttf")) { Color = SKColors.Black, TextSize = 32 };
                    break;
            }
            PaintStore[key] = paint;
            return paint;
        }

        public TextProperties GetTextProperties(Models.Style style)
        {
            var key = style.ToString();
            if (PropertiesStore.TryGetValue(key, out var properties))
            {
                return properties;
            }
            switch (style)
            {
                case Models.Style.Paragraph:
                    properties = new TextProperties { Size = 20, LineHeight = 1.25f, Margin = new SKRect(5, 5, 5, 5), Offset = -5, Spacing = 0, Indent = 1 };
                    break;
                case Models.Style.Header:
                    properties = new TextProperties { Size = 32, LineHeight = 1.25f, Margin = new SKRect(5, 5, 5, 5), Offset = -5, Spacing = 0, Indent = 0 };
                    break;
            }
            PropertiesStore[key] = properties;
            return properties;
        }
    }
}
