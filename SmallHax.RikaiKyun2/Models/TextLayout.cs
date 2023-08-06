using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace SmallHax.RikaiKyun2.Models
{
    public class TextLayout
    {
        //public float X { get; set; }
        public float Y { get; set; }
        public float Bottom => Y + Height;
        private float _width;
        public float Width { get { return _width; } set { _width = value; UpdateLayout(); } }
        public float Height { get; private set; }
        public List<CharacterData> Characters { get; private set; } = new List<CharacterData>();
        public TextNode Node { get; }
        public TextProperties TextProperties { get; }

        public TextLayout(TextNode node, float width, TextProperties textProperties)
        {
            Node = node;
            Characters = Node.Text.Select((x, i) => new CharacterData { Character = x.ToString(), Index = i }).ToList();
            TextProperties = textProperties;
            Width = width;
        }

        private void UpdateLayout()
        {
            if (Width == 0)
            {
                return;
            }
            if (Node.Text.Length == 0)
            {
                Height = 0;
                return;
            }
            var fontSize = TextProperties.Size;
            var i = 0;
            var x = 0f;
            var y = 0f;
            var maxWidth = Width - fontSize - TextProperties.Margin.Left - TextProperties.Margin.Right;
            var characterWidth = fontSize * (1f + TextProperties.Spacing);
            var lineHeight = fontSize * TextProperties.LineHeight;
            while (i < Node.Text.Length)
            {
                var characterData = Characters[i];
                var canFit = x == 0 || x + characterWidth < maxWidth;
                if (!canFit)
                {
                    x = 0f;
                    y += lineHeight;
                    continue;
                }
                if (i == 0)
                {
                    x += characterWidth * TextProperties.Indent;
                }
                var characterX = x + TextProperties.Margin.Left;
                var characterY = y + TextProperties.Margin.Top;
                characterData.Rect = new SKRect(characterX, characterY, characterX + characterWidth, characterY + lineHeight);
                characterData.Origin = new SKPoint(characterData.Rect.Left, characterData.Rect.Bottom + TextProperties.Offset);

                x += characterWidth;
                i++;
            }
            Height = Characters.Last().Rect.Bottom + TextProperties.Margin.Bottom;
        }
    }
}
