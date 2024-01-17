using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MaterialDesign3.Converters
{
    internal class CornerRadiusFull : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double h)
            {
                return new CornerRadius(h / 2);
            }
            else
            {
                return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
