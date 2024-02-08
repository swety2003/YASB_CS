using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using APP.Models;
using APP.Models.Enums;
using APP.Shared.Core;
using Avalonia.Controls;

namespace APP.Common;

internal class WidgetContainerService
{
    internal StackPanel? LeftPanel { get; set; }

    internal StackPanel? CenterPanel { get; set; }

    internal StackPanel? RightPanel { get; set; }

    private Dictionary<WidgetPosition, StackPanel> PanelMap { get; } = new();

    private IList<WidgetStatus> topBarStatusList { get; } = new List<WidgetStatus>();

    internal IList<WidgetStatus> WidgetStatusList => topBarStatusList;

    private IList<IWidgetItem> activeItems { get; } = new List<IWidgetItem>();

    public void InitTopBarContainerService()
    {
        PanelMap[WidgetPosition.Left] = LeftPanel ?? throw new Exception();
        PanelMap[WidgetPosition.Center] = CenterPanel ?? throw new Exception();
        PanelMap[WidgetPosition.Right] = RightPanel ?? throw new Exception();

        foreach (var item in Program.GetService<PluginLoader>().WidgetMainfests)
            topBarStatusList.Add(new WidgetStatus(item.Widget.FullName ?? throw new Exception()));
    }


    internal event PropertyChangedEventHandler? PropertyChanged;

    internal void Enable(WidgetStatus ts, WidgetPosition pos)
    {
        var card = Activator.CreateInstance(ts.WidgetMainfests.Widget) as IWidgetItem;

        PanelMap[pos].Children.Add(card as UserControl ?? throw new Exception());
        activeItems.Add(card);

        card.OnEnabled();

        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(WidgetStatusList)));
    }


    public void Disable(WidgetStatus tbs)
    {
        try
        {
            var c = activeItems.Where(x => x.Info == tbs.WidgetMainfests).First();
            Disable(c, tbs.Pos);
        }
        catch (Exception ex)
        {
            //logger.LogError(ex, "禁用失败");
        }
    }

    public void Disable(IWidgetItem c, WidgetPosition pos)
    {
        c.OnDisabled();

        PanelMap[pos].Children.Remove(c as UserControl);

        activeItems.Remove(c);

        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(WidgetStatusList)));
    }
}