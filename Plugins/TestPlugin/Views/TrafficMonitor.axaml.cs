using APP.Shared;
using APP.Shared.Controls;
using Avalonia.Controls;
using TestPlugin.ViewModels;

namespace TestPlugin.Views;
[Widget("资源监视", "23333")]

public partial class TrafficMonitor : WidgetControl<TrafficMonitorViewModel>
{
    public TrafficMonitor()
    {
        InitializeComponent();
    }
}