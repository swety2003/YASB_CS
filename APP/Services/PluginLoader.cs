using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using APP.Shared;
using Microsoft.Extensions.Logging;

namespace APP.Services;

internal class PluginLoader
{
    private readonly ILogger<PluginLoader> _logger;

    /// <summary>
    ///     可用插件列表
    /// </summary>
    public IEnumerable<PluginInfo> Plugins = new ObservableCollection<PluginInfo>();

    /// <summary>
    ///     已经加载的顶栏项目信息
    /// </summary>
    public ObservableCollection<WidgetMainfest> WidgetMainfests = new();

    public PluginLoader(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<PluginLoader>();
        Logger.LoggerFactory = loggerFactory;
    }


    private PluginInfo CreatePluginInstances(Assembly assembly)
    {
        return new PluginInfo(assembly);
    }

    private Assembly LoadPlugin(string pluginLocation)
    {
        _logger.LogDebug($"加载插件: {Path.GetFileName(pluginLocation)}");
        var loadContext = new PluginLoadContext(pluginLocation);
        return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
    }

    public void Load()
    {
        _logger.LogDebug("开始加载插件");

        var ROOT_FOLDER = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ??
                          throw new Exception("获取软件路径失败！");

        var PLUGIN_FOLDER = Path.Combine(ROOT_FOLDER, "Plugins");

        if (!Directory.Exists(PLUGIN_FOLDER)) Directory.CreateDirectory(PLUGIN_FOLDER);

        var plugin_folders = Directory.GetDirectories(PLUGIN_FOLDER);

        var Plugins = plugin_folders.Select(pluginPath =>
        {
            var entery = new DirectoryInfo(pluginPath).Name + ".dll";
            var plugin_main = Path.Combine(pluginPath, entery);
            var pluginAssembly = LoadPlugin(plugin_main);
            return CreatePluginInstances(pluginAssembly);
        }).ToList();


        _logger.LogDebug($"找到了{Plugins.Count}个插件");

        foreach (var item in Plugins)
            try
            {
                foreach (var c in item.GetAllTypeInfo())
                    if (c is WidgetMainfest sbinfo)
                        WidgetMainfests.Add(sbinfo);
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"加载{item.name}时发生错误：{ex.Message}");
            }
    }
}

internal class PluginLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;

    public PluginLoadContext(string pluginPath)
    {
        if (!File.Exists(pluginPath)) throw new FileNotFoundException(pluginPath);
        _resolver = new AssemblyDependencyResolver(pluginPath);
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        var assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
        if (assemblyPath != null) return LoadFromAssemblyPath(assemblyPath);

        return null;
    }

    protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
    {
        var libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
        if (libraryPath != null) return LoadUnmanagedDllFromPath(libraryPath);

        return IntPtr.Zero;
    }
}