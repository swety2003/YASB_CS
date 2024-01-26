using APP.SDK.Core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TestPlugin.ViewModels;

namespace TestPlugin.Views;

public partial class PowerStatus : UserControl,IWidgetItem
{
    internal static WidgetMainfest info = new WidgetMainfest("电源状态", "", typeof(PowerStatus));
    private PowerStatusViewModel vm;
    public PowerStatus()
    {
        InitializeComponent();
        vm = new PowerStatusViewModel(this);
        DataContext = vm;
    }

    public void OnEnabled()
    {
        vm.OnEnabled();
    }

    public void OnDisabled()
    {
        vm.OnDisabled();
    }

    public WidgetMainfest Info => info;
}