using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using APP.Models.Enums;
using APP.Services;
using APP.Shared;
using APP.Shared.Extra;
using Newtonsoft.Json;
using static APP.Services.ServiceManager;

namespace APP.Models;

[DebuggerDisplay("{Mainfest.Widget.Name},{enabledProperty}")]
public class WidgetProfile
{
    [JsonProperty("Enabled")] private bool enabledProperty;

    private WidgetPosition posProperty = WidgetPosition.Left;

    public WidgetProfile(string wid, WidgetPosition pos = WidgetPosition.Left)
    {
        Wid = wid;
        Pos = pos;
    }

    [JsonProperty("WID")] public string Wid { get; private set; }

    public WidgetPosition Pos
    {
        get => posProperty;
        set
        {
            if (Enabled)
            {
                // GetService<WidgetManager>().Disable(this);
                // GetService<WidgetManager>().Enable(this, value);
                GetService<WidgetManager>().ChangePos(this,posProperty,value);
            }

            posProperty = value;
        }
    }

    [JsonIgnore]
    public IEnumerable<EnumDescription> PosNameList =>
        Enum.GetValues(typeof(WidgetPosition))
            .Cast<WidgetPosition>()
            .Select(e => new EnumDescription(e));

    [JsonIgnore]
    public bool Enabled
    {
        get => enabledProperty;
        set
        {
            enabledProperty = value;
            if (value)
                GetService<WidgetManager>().Enable(this, Pos);
            else
                GetService<WidgetManager>().Disable(this);
        }
    }

    [JsonIgnore]
    public WidgetMainfest Mainfest => GetService<PluginLoader>().WidgetMainfests
        .Where(x => x.Widget.FullName == Wid).Select(x => x).First();
    
    
    internal void OpenPluginFolder()
    {
        var p = this.Mainfest.PluginInfo.GetLocation();
        if (Directory.Exists(p)) Process.Start("explorer.exe", p);
    }
}