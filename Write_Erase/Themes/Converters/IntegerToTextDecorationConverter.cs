using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Write_Erase.Themes.Converters
{
    internal class IntegerToTextDecorationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var integer = value as int?;
            if (integer == null || integer == 0) return default;
            else return TextDecorations.Strikethrough;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
