using Microsoft.Extensions.Logging;
using PluginSDK;
using PluginSDK.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace YASB_CS.Common
{

    public class PluginLoader
    {
        private readonly ILogger<PluginLoader> _logger;

        public PluginLoader(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PluginLoader>();
            Logger.LoggerFactory = loggerFactory;
        }

        /// <summary>
        /// 可用插件列表
        /// </summary>
        public IEnumerable<IPlugin> Plugins = new ObservableCollection<IPlugin>();

        /// <summary>
        /// 已经加载的顶栏项目信息
        /// </summary>
        public ObservableCollection<TopBarItemInfo> TopBarItemInfos = new ObservableCollection<TopBarItemInfo>();


        IEnumerable<IPlugin> CreatePluginInstances(Assembly assembly)
        {
            int count = 0;

            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetInterface("IPlugin") != null)
                {
                    IPlugin? result = Activator.CreateInstance(type) as IPlugin;
                    if (result != null)
                    {
                        count++;
                        yield return result;
                    }
                }
            }

            if (count == 0)
            {
                string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));

                _logger.LogWarning($"Can't find any type which implements IPlugin in {assembly} from {assembly.Location}.\n" +
                    $"Available types: {availableTypes}");
            }
        }

        Assembly LoadPlugin(string pluginLocation)
        {

            _logger.LogDebug($"加载插件: {Path.GetFileName(pluginLocation)}");
            PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
            return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
        }

        public void Load()
        {
            _logger.LogDebug($"开始加载插件");

            string ROOT_FOLDER = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) ?? throw new Exception("获取软件路径失败！");

            string PLUGIN_FOLDER = Path.Combine(ROOT_FOLDER, "Plugins");

            if (!Directory.Exists(PLUGIN_FOLDER))
            {
                Directory.CreateDirectory(PLUGIN_FOLDER);
            }

            string[] plugin_folders = Directory.GetDirectories(PLUGIN_FOLDER);


            var Plugins = plugin_folders.SelectMany(pluginPath =>
            {
                var entery = new DirectoryInfo(pluginPath).Name + ".dll";
                string plugin_main = Path.Combine(pluginPath, entery);
                Assembly pluginAssembly = LoadPlugin(plugin_main);
                return CreatePluginInstances(pluginAssembly);

            }).ToList();


            _logger.LogDebug($"找到了{Plugins.Count}个插件");

            foreach (var item in Plugins)
            {
                try
                {
                    foreach (var c in item.GetAllTypeInfo())
                    {
                        if (c is TopBarItemInfo sbinfo)
                        {

                            TopBarItemInfos.Add(sbinfo);
                        }
                    }
                }
                catch (Exception ex)
                {

                    _logger.LogWarning($"加载{item.name}时发生错误：{ex.Message}");

                }

            }


        }

    }

    class PluginLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver _resolver;

        public PluginLoadContext(string pluginPath)
        {
            _resolver = new AssemblyDependencyResolver(pluginPath);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);
            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
    }
}
