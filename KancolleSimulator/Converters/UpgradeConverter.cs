using System;
using System.Globalization;
using System.Windows.Data;

namespace KancolleSimulator.Converters
{
    public class UpgradeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value switch
            {
                10 => "MAX",

                _ => $"+{value}"
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}