using Avalonia.Controls;
using YASB.ViewModels;

namespace YASB;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        if (!Design.IsDesignMode) DataContext = Program.GetService<SettingsWindowViewModel>();
    }
}