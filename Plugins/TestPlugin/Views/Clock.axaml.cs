using APP.SDK.Core;
using Avalonia.Controls;
using TestPlugin.ViewModels;

namespace TestPlugin.Views;

public partial class Clock : UserControl, IWidgetItem
{
    internal static WidgetMainfest info = new("???????", "", typeof(Clock));
    private readonly ClockViewModel vm;

    public Clock()
    {
        InitializeComponent();
        vm = new ClockViewModel(this);
        DataContext = vm;
    }

    public WidgetMainfest Info => info;

    public void OnDisabled()
    {
        vm.OnDisabled();
    }

    public void OnEnabled()
    {
        vm.OnEnabled();
    }
}