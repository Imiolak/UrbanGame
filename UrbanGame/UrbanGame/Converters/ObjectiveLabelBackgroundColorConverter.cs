using System;
using System.Globalization;
using Xamarin.Forms;

namespace UrbanGame.Converters
{
    public class ObjectiveLabelBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value
                ? Color.Green
                : Color.Orange;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
