using APP.SDK.Core;
using Avalonia.Controls;
using TestPlugin.ViewModels;

namespace TestPlugin.Views;

public partial class TrafficMonitor : UserControl, IWidgetItem
{
    internal static WidgetMainfest info = new("资源监视器", "", typeof(TrafficMonitor));
    private readonly TrafficMonitorViewModel _viewModel;

    public TrafficMonitor()
    {
        InitializeComponent();
        _viewModel = new TrafficMonitorViewModel(this);
        DataContext = _viewModel;
    }

    public void OnEnabled()
    {
        _viewModel.OnEnabled();
    }

    public void OnDisabled()
    {
        _viewModel.OnDisabled();
    }

    public WidgetMainfest Info => info;
}