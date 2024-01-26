using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using APP.SDK;
using APP.SDK.Core;
using Microsoft.Extensions.Logging;

namespace YASB.Common;

internal class PluginLoader
{
    private readonly ILogger<PluginLoader> _logger;

    /// <summary>
    ///     可用插件列表
    /// </summary>
    public IEnumerable<IPlugin> Plugins = new ObservableCollection<IPlugin>();

    /// <summary>
    ///     已经加载的顶栏项目信息
    /// </summary>
    public ObservableCollection<WidgetMainfest> WidgetMainfests = new();

    public PluginLoader(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<PluginLoader>();
        Logger.LoggerFactory = loggerFactory;
    }


    private IEnumerable<IPlugin> CreatePluginInstances(Assembly assembly)
    {
        var count = 0;

        foreach (var type in assembly.GetTypes())
            if (type.GetInterface("IPlugin") != null)
            {
                var result = Activator.CreateInstance(type) as IPlugin;
                if (result != null)
                {
                    count++;
                    yield return result;
                }
            }

        if (count == 0)
        {
            var availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));

            _logger.LogWarning(
                $"Can't find any type which implements IPlugin in {assembly} from {assembly.Location}.\n" +
                $"Available types: {availableTypes}");
        }
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

#if DEBUG
        var pfl = plugin_folders.ToList();
        pfl.Add("D:\\Repo\\CS\\YASB-CS\\build\\Plugins\\Debug\\TestPlugin");
        plugin_folders = pfl.ToArray();
#endif


        var Plugins = plugin_folders.SelectMany(pluginPath =>
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