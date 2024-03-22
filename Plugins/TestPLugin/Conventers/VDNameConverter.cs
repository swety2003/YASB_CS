using System;
using System.Globalization;
using Avalonia.Data.Converters;
using TestPlugin.ViewModels;
using WindowsDesktop;

namespace TestPlugin.Conventers;

public class VDNameConverter:IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is VirtualDesktop vd && parameter is VirtualDesktopManagerViewModel vm)
        {
            
        }
        return value;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string name && parameter is VirtualDesktopManagerViewModel vm)
        {
            
        }
        return null;
    }
}