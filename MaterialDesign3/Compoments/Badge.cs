using System.Windows;
using System.Windows.Controls;

namespace MaterialDesign3.Compoments;

/// <summary>
///     标记
/// </summary>
public class Badge : ContentControl
{
    private int? _originalValue;


    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text), typeof(string), typeof(Badge), new PropertyMetadata("0"));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
        nameof(Value), typeof(int), typeof(Badge), new PropertyMetadata(0, OnValueChanged));

    private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (Badge)d;
        var v = (int)e.NewValue;
        ctl.SetCurrentValue(TextProperty, v <= ctl.Maximum ? v.ToString() : $"{ctl.Maximum}+");
        if (ctl.IsLoaded)
        {
            //ctl.RaiseEvent(new FunctionEventArgs<int>(ValueChangedEvent, ctl)
            //{
            //    Info = v
            //});
        }
        else
        {
            ctl._originalValue = v;
        }
    }

    public int Value
    {
        get => (int)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }


    public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
        nameof(Maximum), typeof(int), typeof(Badge), new PropertyMetadata(99, OnMaximumChanged));

    private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var ctl = (Badge)d;
        var v = ctl.Value;
        ctl.SetCurrentValue(TextProperty, v <= ctl.Maximum ? v.ToString() : $"{ctl.Maximum}+");
    }

    public int Maximum
    {
        get => (int)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly DependencyProperty BadgeMarginProperty = DependencyProperty.Register(
        nameof(BadgeMargin), typeof(Thickness), typeof(Badge), new PropertyMetadata(default(Thickness)));

    public Thickness BadgeMargin
    {
        get => (Thickness)GetValue(BadgeMarginProperty);
        set => SetValue(BadgeMarginProperty, value);
    }

    public static readonly DependencyProperty BadgeVisibilityProperty = DependencyProperty.Register(
        nameof(BadgeVisibility), typeof(Visibility), typeof(Badge), new PropertyMetadata(default(Visibility)));

    public Visibility BadgeVisibility
    {
        get => (Visibility)GetValue(BadgeVisibilityProperty);
        set => SetValue(BadgeVisibilityProperty, value);
    }


}
