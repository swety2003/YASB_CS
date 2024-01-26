using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YASB.Common;
using YASB.ViewModels;

namespace YASB;

internal sealed class Program
{
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
                services.AddSingleton<AppConfigService>();
                services.AddSingleton<WidgetContainerService>();
                services.AddSingleton<PluginLoader>();

                #region ViewModel

                services.AddSingleton<SettingsWindowViewModel>();
                //services.AddSingleton<ItemManageVM>();
                //services.AddSingleton<CardManageVM>();
                //services.AddSingleton<InstalledCardsVM>();
                //services.AddSingleton<PreferenceVM>();
                //services.AddSingleton<SideBarManageVM>();

                #endregion

                //services.AddTransient<SettingWindow>();
                //services.AddTransient<CardManage>();
                //services.AddTransient<PreferencePage>();

                //services.AddTransient<Settings>();
                //services.AddTransient<AboutPage>();
                //services.AddTransient<InstalledCards>();
                //services.AddTransient<SideBarManage>();
            }).Build();
    }

    private static IHost AppHost { get; }

    public static T GetService<T>() where T : class
    {
        if (AppHost.Services.GetService(typeof(T)) is not T service)
            throw new ArgumentException($"{typeof(T)} Service Not Found");
        return service;
    }


    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            
            bool createNew;
            var _ = new Mutex(true, "swety.yasb.app", out createNew);
            if (!createNew)
                //MessageBox.Show("Application is already run!");
                Environment.Exit(0);


            Program.GetService<AppConfigService>().Load();
            Program.GetService<PluginLoader>().Load();
            
            // 在此处准备和运行您的 App
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            Debugger.Break();
            // 在这里我们可以处理异常，例如将其添加到日志文件中
            //Log.Fatal(e, "发生了一些非常糟糕的事情");
        }
        finally
        {
            // 此块是可选的。
            // 如果需要清理或类似操作，请使用 finally 块
            //Log.CloseAndFlush();
            Program.GetService<AppConfigService>().Config.Status =
                Program.GetService<SettingsWindowViewModel>().TopBarStatuses.ToList();
            Program.GetService<AppConfigService>().Save();
            foreach (var item in Program.GetService<AppConfigService>().Config.Status) item.Enabled = false;
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