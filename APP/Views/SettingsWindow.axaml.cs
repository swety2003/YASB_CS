using APP.Shared;
using APP.ViewModels;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using static APP.Services.ServiceManager;

namespace APP;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        if (!Design.IsDesignMode) DataContext = GetService<SettingsWindowViewModel>();
        Events.OnRequestExit += (_, _) => Close();
    }
}