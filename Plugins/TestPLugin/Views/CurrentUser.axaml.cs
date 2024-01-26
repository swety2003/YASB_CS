using APP.SDK.Core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TestPlugin.Extra;
using TestPlugin.ViewModels;

namespace TestPlugin.Views;

public partial class CurrentUser : UserControl,IWidgetItem
{
    public CurrentUser()
    {
        InitializeComponent();
        this.InitVM<CurrentUserViewModel>();
    }

    public void OnEnabled()=> this.GetVM<CurrentUserViewModel>().OnEnabled();

    public void OnDisabled()=>this.GetVM<CurrentUserViewModel>().OnDisabled();
    public WidgetMainfest Info => info;
    internal static WidgetMainfest info = new("User", "", typeof(CurrentUser));

}