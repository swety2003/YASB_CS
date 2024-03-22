using APP.Shared;
using APP.Shared.Controls;
using TestPlugin.ViewModels;

namespace TestPlugin.Views;

[Widget("时钟", "23333")]
public partial class Clock : WidgetControl<ClockViewModel>
{
    public Clock()
    {
        InitializeComponent();
    }
}