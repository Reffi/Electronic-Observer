using System;
using System.Globalization;
using System.Windows.Data;

namespace KancolleSimulator.Converters
{
    class ProficiencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value switch
            {
                7 => ">>",
                6 => @"\\\",
                5 => @"\\",
                4 => @"\",
                3 => "|||",
                2 => "||",
                1 => "|",

                _ => ""
            };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
