using System;
using System.ComponentModel;

namespace APP.Shared.Extra;

public class EnumDescription
{
    public EnumDescription(Enum value)
    {
        Value = value;
        Description = GetEnumDescription(value);
    }

    public string? Description { get; }
    public Enum Value { get; }

    private static string? GetEnumDescription(object value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString() ?? string.Empty);
        var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes.Length > 0 ? attributes[0].Description : value.ToString();
    }
}