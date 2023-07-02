using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmallHax.RikaiKyun2.Services
{
    public class FontService
    {
        private Dictionary<string,SKFont> Fonts { get; set; } = new Dictionary<string, SKFont>();

        public SKFont GetFont(string name)
        {
            if (!Fonts.TryGetValue(name, out var font))
            {
                var assembly = Assembly.GetExecutingAssembly();
                using var stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Resources.Fonts." + name);
                var typeface = SKTypeface.FromStream(stream);
                font = new SKFont { Typeface = typeface };
                Fonts[name] = font;
            }
            return font;
        }
    }
}
