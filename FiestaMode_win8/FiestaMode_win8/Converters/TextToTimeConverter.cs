using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace FiestaMode.Converters
{
    public sealed class TextToTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string culture)
        {
            string val = (string)value;
            string min = int.Parse( val.Substring(0, 2) ).ToString("d2");
            string sec = int.Parse(val.Substring(2, 2)).ToString("d2");

            return min + ":" + sec;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string culture)
        {
            return value;// Binding.DoNothing;
        }
    }
}
