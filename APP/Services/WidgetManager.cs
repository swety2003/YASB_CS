using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using APP.Models;
using APP.Models.Enums;
using APP.Shared;
using Avalonia.Controls;
using Microsoft.Extensions.Logging;
using static APP.Services.ServiceManager;


namespace APP.Services;

internal class WidgetManager
{
    private ILogger<WidgetManager> _logger { get; }

    public WidgetManager(ILogger<WidgetManager> logger)
    {
        _logger = logger;
    }
    internal StackPanel? LeftPanel { get; set; }

    internal StackPanel? CenterPanel { get; set; }

    internal StackPanel? RightPanel { get; set; }

    private Dictionary<WidgetPosition, StackPanel> PanelMap { get; } = new();

    private IList<WidgetProfile> topBarStatusList { get; } = new List<WidgetProfile>();

    internal IList<WidgetProfile> WidgetStatusList => topBarStatusList;

    private Dictionary<Control,WidgetProfile> activeItems { get; } = new();

    public void Init()
    {
        PanelMap[WidgetPosition.Left] = LeftPanel ?? throw new Exception();
        PanelMap[WidgetPosition.Center] = CenterPanel ?? throw new Exception();
        PanelMap[WidgetPosition.Right] = RightPanel ?? throw new Exception();

        foreach (var item in GetService<PluginLoader>().WidgetMainfests)
            topBarStatusList.Add(new WidgetProfile(item.Widget.FullName ?? throw new Exception()));
    }

    public List<WidgetProfile> GetSortedProfiles()
    {
        List<WidgetProfile> ret = new();
        foreach (var panel in PanelMap.Values)
        {
            foreach (var c in panel.Children)
            {
                ret.Add(activeItems[c]);
            }
        }

        return ret;
    }

    public void ChangePos(WidgetProfile m, WidgetPosition old, WidgetPosition value)
    {
        foreach (var item in activeItems)
        {
            if (item.Value==m)
            {
                PanelMap[old].Children.Remove(item.Key);
                PanelMap[value].Children.Add(item.Key);
                break;
            }
        }
    }

    internal event PropertyChangedEventHandler? PropertyChanged;

    internal void Enable(WidgetProfile ts, WidgetPosition pos)
    {
        var card = Activator.CreateInstance(ts.Mainfest.Widget) as Control;
        PanelMap[pos].Children.Add(card as UserControl ?? throw new Exception());
        activeItems.Add(card,ts);
        
        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(WidgetStatusList)));

        _logger.LogDebug($"Enabled {ts.Wid},position is {pos}.");
        
    }


    public void Disable(WidgetProfile tbs)
    {
        try
        {
            var c = activeItems.Where(x => x.Key.GetType() == tbs.Mainfest.Widget).First();
            Disable(c.Key, tbs.Pos);
        }
        catch (Exception ex)
        {
            //logger.LogError(ex, "禁用失败");
        }
    }

    public void Disable(Control c, WidgetPosition pos)
    {
        PanelMap[pos].Children.Remove(c);

        activeItems.Remove(c);

        PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(WidgetStatusList)));
        _logger.LogDebug($"Disabled {c.GetType()},position is {pos}.");

    }
}