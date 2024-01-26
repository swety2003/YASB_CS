using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TestPlugin.ViewModels;

public partial class CurrentUserViewModel:ViewModelBase
{
    [ObservableProperty]
    private string userName;

    [ObservableProperty]
    private string machineName;

    public CurrentUserViewModel(UserControl control) : base(control)
    {
        UserName = Environment.UserName;
        machineName = Environment.MachineName;
    }
    internal override void OnEnabled()
    {
    }

    internal override void OnDisabled()
    {
    }
    
    
}