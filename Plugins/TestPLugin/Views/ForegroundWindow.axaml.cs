using APP.Shared;
using APP.Shared.Controls;
using TestPlugin.ViewModels;

namespace TestPlugin.Views;
[Widget("前台窗口", "23333")]

public partial class ForegroundWindow : WidgetControl<ForegroundWindowViewModel>
{
    public ForegroundWindow()
    {
        InitializeComponent();
    }
}