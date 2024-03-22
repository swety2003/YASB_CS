using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace APP.Shared;

[AttributeUsage(AttributeTargets.Class)]
public class WidgetAttribute : Attribute
{
    public WidgetAttribute(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public string Name { get; set; }
    public string Description { get; set; }
}


public record WidgetMainfest(string Name, string Description, Type Widget, PluginInfo PluginInfo);

public class PluginInfo
{
    private readonly Assembly assembly;

    public PluginInfo(Assembly assembly)
    {
        name = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? "";
        author = assembly.GetCustomAttribute<AssemblyCompanyAttribute>()?.Company ?? "";
        desc = assembly.GetCustomAttribute<AssemblyDescriptionAttribute>()?.Description ?? "";
        version = assembly.GetName().Version ?? new Version();
        this.assembly = assembly;
    }

    public string name { get; init; }
    public string desc { get; init; }
    public Version version { get; init; }
    public string url { get; init; }
    public string author { get; init; }

    public string? GetLocation()
    {
        try
        {
            return new DirectoryInfo(Path.GetDirectoryName(assembly.Location)).ToString();
        }
        catch (Exception)
        {
            return string.Empty;
        }
    }

    public IEnumerable<WidgetMainfest> GetAllTypeInfo()
    {
        var ret = new List<WidgetMainfest>();
        List<(Type widget, WidgetAttribute? WidgetInfo)> widgetTypes = assembly.GetTypes()
            .Where(type => type.IsDefined(typeof(WidgetAttribute), false))
            .Select(type =>
            {
                var attribute = type.GetCustomAttribute<WidgetAttribute>();
                return (type, attribute);
            })
            .ToList();
        foreach (var attr in widgetTypes)
        {
            if (attr.WidgetInfo == null) continue;
            ret.Add(new WidgetMainfest(attr.WidgetInfo.Name, attr.WidgetInfo.Description, attr.widget, this));
        }

        return ret;
    }
}

/// <summary>
/// </summary>
public static class Logger
{
    private static ILoggerFactory? _loggerFactory;

    public static ILoggerFactory? LoggerFactory
    {
        get => _loggerFactory;
        set
        {
            if (_loggerFactory == null) _loggerFactory = value;
        }
    }

    public static ILogger<T> CreateLogger<T>()
    {
        return LoggerFactory?.CreateLogger<T>() ?? throw new Exception("Logger.LoggerFactory 未初始化！");
    }
}

public static class Events
{

    public static EventHandler? OnRequestExit;
}