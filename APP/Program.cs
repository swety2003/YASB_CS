using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using APP.Common;
using APP.Models;
using APP.Services;
using APP.ViewModels;
using APP.Views;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static APP.Services.ServiceManager;

namespace APP;

internal sealed class Program
{

    private static ILogger<Program> _logger;

    #region APPHost init

    
    static Program()
    {
        AppHost = Host.CreateDefaultBuilder()
            .ConfigureLogging(logging =>
            {
                logging.ClearProviders()
                    .AddSimpleConsole(options =>
                    {
                        options.SingleLine = true;
                        options.IncludeScopes = true;
                        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
                        options.UseUtcTimestamp = false;
                    }).AddDebug();

                logging.SetMinimumLevel(LogLevel.Debug);
            })
            .UseContentRoot(AppContext.BaseDirectory).ConfigureServices
            ((context, services) =>
            {
                var allTypes = Assembly.GetExecutingAssembly().GetTypes();

                // 过滤出特定命名空间下的类型（类）
                var svs = allTypes.Where(type =>
                    type.Namespace == "APP.Services" && !(type.IsAbstract && type.IsSealed && type.IsClass));
                foreach (var type in svs) services.AddSingleton(type);

                #region ViewModel

                var viewmodels = allTypes.Where(type => type.Namespace == "APP.ViewModels");
                foreach (var type in viewmodels) services.AddSingleton(type);

                #endregion
                var views = allTypes.Where(type => type.Namespace == "APP.Views");
                foreach (var type in views) services.AddTransient(type);

                //services.AddTransient<CardManage>();
                //services.AddTransient<PreferencePage>();

                //services.AddTransient<Settings>();
                //services.AddTransient<AboutPage>();
                //services.AddTransient<InstalledCards>();
                //services.AddTransient<SideBarManage>();
            }).Build();
    }

    #endregion

    internal static IHost AppHost { get; }

    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            _logger = GetService<ILoggerFactory>().CreateLogger<Program>();

            bool createNew;
            string n = typeof(Program).Namespace;
            _ = new Mutex(true, $"swety.yasb.app-{n}", out createNew);
            if (!createNew)
                //MessageBox.Show("Application is already run!");
                Environment.Exit(0);


            GetService<AppConfigManager>().Load();
            GetService<PluginLoader>().Load();

            // 在此处准备和运行您的 App
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);

#if DEBUG
            throw;
#endif

        }
        finally
        {
            
            var ac = GetService<WidgetManager>().GetSortedProfiles();

            GetService<AppConfigManager>().Config.Status = ac;
            GetService<AppConfigManager>().Save();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
    }
}