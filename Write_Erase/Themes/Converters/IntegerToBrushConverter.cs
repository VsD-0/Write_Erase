using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Write_Erase.Themes.Converters
{
    internal class IntegerToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var integer = value as int?;
            if (integer == null) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#76E383"));
            else if (integer >= 9) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7FFF00"));
            else return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#76E383"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
