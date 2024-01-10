using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using YASB_CS.Common;
using YASB_CS.View;
using YASB_CS.ViewModel;

namespace YASB_CS
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        static IHost AppHost
        {
            get;
        }
        public static T GetService<T>() where T : class
        {
            if (AppHost.Services.GetService(typeof(T)) is not T service)
            {
                throw new ArgumentException($"{typeof(T)} Service Not Found");
            }
            return service;
        }

        static App()
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
                    });
                    logging.SetMinimumLevel(LogLevel.Debug);

                })
                .UseContentRoot(AppContext.BaseDirectory).ConfigureServices
                ((context, services) =>
                {
                    services.AddSingleton<AppConfigManager>();
                    services.AddSingleton<TopBarContainerService>();
                    services.AddSingleton<PluginLoader>();

                    #region ViewModel

                    services.AddSingleton<MainWindowVM>();
                    services.AddSingleton<ItemManageVM>();
                    //services.AddSingleton<CardManageVM>();
                    //services.AddSingleton<InstalledCardsVM>();
                    //services.AddSingleton<PreferenceVM>();
                    //services.AddSingleton<SideBarManageVM>();

                    #endregion
                    services.AddTransient<SettingWindow>();
                    //services.AddTransient<CardManage>();
                    //services.AddTransient<PreferencePage>();

                    //services.AddTransient<Settings>();
                    //services.AddTransient<AboutPage>();
                    //services.AddTransient<InstalledCards>();
                    //services.AddTransient<SideBarManage>();






                }).Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            bool createNew;
            Mutex mutex = new Mutex(true, "swety.yasbcs.app", out createNew);
            if (!createNew)
            {
                MessageBox.Show("Application is already run!");
                Shutdown();
            }
            Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;



            App.GetService<AppConfigManager>().Load();
            App.GetService<PluginLoader>().Load();


            base.OnStartup(e);
        }


        protected override void OnExit(ExitEventArgs e)
        {
            //Messages.SendOnExitMsg();

            base.OnExit(e);
            GetService<AppConfigManager>().Config.Status = GetService<ItemManageVM>().TopBarStatuses.ToList();
            GetService<AppConfigManager>().Save();

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
#if !DEBUG
            MessageBox.Show($"我们很抱歉，当前应用程序遇到一些问题，该操作已经终止:{e.Exception.Message}", "意外的操作", MessageBoxButton.OK, MessageBoxImage.Information);//这里通常需要给用户一些较为友好的提示，并且后续可能的操作

            File.WriteAllText("err.log", JsonConvert.SerializeObject(e.Exception));

            e.Handled = true;
#endif
        }
    }



}
