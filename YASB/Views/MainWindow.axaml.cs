using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using YASB.Common;
using YASB.Common.WindowHelper;
using YASB.Models;
using YASB.ViewModels;

namespace YASB.Views;

public partial class MainWindow : Window
{
    private readonly DesktopAppBar dab_helper;

    public MainWindow()
    {
        InitializeComponent();

        dab_helper = new DesktopAppBar(this);
    }

    private void MenuItem_Click(object? sender, RoutedEventArgs e)
    {
        Close();
        // Environment.Exit(0);
    }

    private void MenuItem_Click1(object? sender, RoutedEventArgs e)
    {
        new SettingsWindow().Show();
    }


    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);

        //var w = new AppBarWindow();
        //w.Show();

        dab_helper.SetAsAppBar();


        new HideInRecentTasksHelper(this).HideInRecentTasks();
        var tbcs = Program.GetService<WidgetContainerService>();
        tbcs.RightPanel = right_area;
        tbcs.LeftPanel = left_area;
        tbcs.CenterPanel = center_area;
        tbcs.InitTopBarContainerService();

        var tbs = Program.GetService<SettingsWindowViewModel>().TopBarStatuses;
        foreach (var item in Program.GetService<AppConfigService>().Config.Status ?? new List<WidgetStatus>())
        foreach (var i1 in tbs)
            if (item.Enabled && i1.Wid == item.Wid)
            {
                i1.Pos = item.Pos;
                i1.Enabled = true;
            }
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        base.OnClosing(e);

        dab_helper.UnSetAppBar();
    }
}