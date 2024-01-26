using APP.SDK.Core;
using Avalonia.Controls;
using TestPlugin.ViewModels;

namespace TestPlugin.Views;

public partial class KomorebiEx : UserControl, IWidgetItem
{
    internal static WidgetMainfest info = new("工作空间管理", "", typeof(KomorebiEx));

    public KomorebiEx()
    {
        InitializeComponent();
        vm = new KomorebiExViewModel(this);
        DataContext = vm;
    }

    public KomorebiExViewModel vm { get; }

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