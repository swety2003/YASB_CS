using System;
using System.Collections.Generic;
using System.Linq;
using APP.Common;
using APP.Models.Enums;
using APP.Shared.Core;
using APP.Shared.Extra;
using Newtonsoft.Json;

namespace APP.Models;

public class WidgetStatus
{
    [JsonProperty("Enabled")] private bool enabledProperty;

    private WidgetPosition posProperty = WidgetPosition.Left;

    public WidgetStatus(string wid, WidgetPosition pos = WidgetPosition.Left)
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
                Program.GetService<WidgetContainerService>().Disable(this);

                Program.GetService<WidgetContainerService>().Enable(this, value);
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
                Program.GetService<WidgetContainerService>().Enable(this, Pos);
            else
                Program.GetService<WidgetContainerService>().Disable(this);
        }
    }

    [JsonIgnore]
    public WidgetMainfest WidgetMainfests => Program.GetService<PluginLoader>().WidgetMainfests
        .Where(x => x.Widget.FullName == Wid).Select(x => x).First();
}