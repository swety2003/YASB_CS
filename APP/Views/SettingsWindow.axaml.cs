using APP.ViewModels;
using Avalonia.Controls;

namespace APP;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        if (!Design.IsDesignMode) DataContext = Program.GetService<SettingsWindowViewModel>();
    }
}