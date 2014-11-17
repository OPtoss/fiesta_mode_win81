using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace FiestaMode.Converters
{
    static class ColorExtensions
    {

        public static string ToHex( this Color c )
        {
            return string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", c.A, c.R, c.G, c.B);
        }

        public static Color ToColor(this uint c)
        {
            byte A = (byte)((c & 0xFF000000) >> 24);
            byte R = (byte)((c & 0x00FF0000) >> 16);
            byte G = (byte)((c & 0x0000FF00) >> 8);
            byte B = (byte)((c & 0x000000FF) >> 0);
            return Color.FromArgb(A, R, G, B);;
        }
    }
}
