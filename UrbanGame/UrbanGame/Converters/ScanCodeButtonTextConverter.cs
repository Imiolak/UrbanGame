using System;
using System.Globalization;
using Xamarin.Forms;

namespace UrbanGame.Converters
{
    public class ScanCodeButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value
                ? "Koniec gry!"
                : "Skanuj kod";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
