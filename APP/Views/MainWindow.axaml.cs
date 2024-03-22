using System;
using System.Threading.Tasks;
using APP.Common;
using APP.Common.WindowHelper;
using APP.Services;
using APP.Shared;
using APP.Shared.Controls;
using APP.ViewModels;
using Avalonia.Interactivity;
using Microsoft.Extensions.Logging;
using static APP.Services.ServiceManager;

namespace APP.Views;

public partial class MainWindow : AppBarWindow
{
    private ILogger<MainWindow> _logger;
    public MainWindow(ILogger<MainWindow> l,MainWindowViewModel vm)
    {
        InitializeComponent();
        _logger = l;
        DataContext = vm;
    }

    private void MenuItem_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void MenuItem_Click1(object? sender, RoutedEventArgs e)
    {
        new SettingsWindow().Show();
    }


    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        Closed += (_, _) => Events.OnRequestExit?.Invoke(this, default); 
        new HideInRecentTasksHelper(this).HideInRecentTasks();
        var tbcs = GetService<WidgetManager>();
        tbcs.RightPanel = right_area;
        tbcs.LeftPanel = left_area;
        tbcs.CenterPanel = center_area;
        tbcs.Init();
        
        _logger.LogDebug($"Load widget from config.");

        var tbs = GetService<SettingsWindowViewModel>().TopBarStatuses;
        foreach (var item in GetService<AppConfigManager>().Config.Status)
        foreach (var i1 in tbs)
            if (item.Enabled && i1.Wid == item.Wid)
            {
                i1.Pos = item.Pos;
                i1.Enabled = true;
            }
        _logger.LogDebug($"Load widget complete.");

    }

}