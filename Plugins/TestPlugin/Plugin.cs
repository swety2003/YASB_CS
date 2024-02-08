using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using APP.Shared;
using APP.Shared.Core;
using Microsoft.Extensions.Logging;
using TestPlugin.Views;

namespace TestPlugin;

public class Plugin : IPlugin
{
    public static List<WidgetMainfest> infos { get; } = new()
    {
        //DevTest.info
        //DigitalClock.info,
        //WorkSpaceManager.info,
        //HardwareMonitor.info,
        //UserInfo.info,
        Clock.info,
        TrafficMonitor.info,
        KomorebiEx.info,
        PowerStatus.info,
        CurrentUser.info,
        VirtualDesktopManager.info,
    };

    public ILoggerFactory LoggerFactory { get; set; }

    public Version version { get; } = new();
    public string url { get; } = "";
    public string author { get; } = "";


    public string name => "233Test";

    public IEnumerable<object> GetAllTypeInfo()
    {
        return infos;
    }

    public static string GetRunningDictionary()
    {
        return Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) ?? throw new NotSupportedException();
    }
}