using APP.Shared;
using APP.Shared.Controls;
using Avalonia.Controls;
using Avalonia.Interactivity;
using KomorebiHelper.ViewModels;

namespace KomorebiHelper.Views;

[Widget("KomorebiUI","")]
public partial class KomorebiView : WidgetControl<KomorebiHelperViewModel>
{
    public KomorebiView()
    {
        InitializeComponent();
    }

    protected override void OnUnloaded(RoutedEventArgs e)
    {
        base.OnUnloaded(e);
        VM.Stop();
    }
}