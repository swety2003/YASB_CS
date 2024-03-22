using APP.Shared;
using APP.Shared.Controls;
using TestPlugin.ViewModels;

namespace TestPlugin.Views;

[Widget("当前用户", "23333")]

public partial class CurrentUser : WidgetControl<CurrentUserViewModel>
{
    public CurrentUser()
    {
        InitializeComponent();
    }
}