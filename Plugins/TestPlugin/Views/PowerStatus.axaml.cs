using APP.Shared;
using APP.Shared.Controls;
using TestPlugin.ViewModels;

namespace TestPlugin.Views;
[Widget("电量", "23333")]

public partial class PowerStatus : WidgetControl<PowerStatusViewModel>
{
    public PowerStatus()
    {
        InitializeComponent();
    }
}