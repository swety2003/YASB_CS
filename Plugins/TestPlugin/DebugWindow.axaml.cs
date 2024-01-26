using System;
using APP.SDK.Core;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace TestPlugin;

public partial class DebugWindow : Window
{
    public DebugWindow()
    {
        InitializeComponent();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);


        var infos = Plugin.infos;
        foreach (var info in infos)
        {
            var uc = Activator.CreateInstance(info.Widget) as UserControl;
            if (uc is IWidgetItem widget)
            {
                container.Children.Add(uc);
                widget.OnEnabled();
            }
        }
    }
}