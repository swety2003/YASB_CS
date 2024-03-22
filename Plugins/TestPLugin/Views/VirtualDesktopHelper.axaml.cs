using System;
using System.IO;
using System.Reflection;
using APP.Shared.Core;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using TestPlugin.Extra;
using TestPlugin.ViewModels;
using WindowsDesktop;

namespace TestPlugin.Views;

public partial class VirtualDesktopHelper : UserControl,IWidgetItem
{
    

    public VirtualDesktopHelper()
    {
        InitializeComObjects();
        InitializeComponent();
        this.InitVM<VirtualDesktopManagerViewModel>();
    }


    void InitializeComObjects()
    {
        var abl = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
        
        AppDomain.CurrentDomain.AssemblyResolve += (_, args) => args.Name.StartsWith("VirtualDesktop")?
            Assembly.GetAssembly(typeof(VirtualDesktop)):null;
    }

    public void OnEnabled()
    {
        this.GetVM<VirtualDesktopManagerViewModel>().OnEnabled();
    }

    public void OnDisabled()
    {        
        this.GetVM<VirtualDesktopManagerViewModel>().OnDisabled();

    }


    public static readonly WidgetMainfest info = new("VD", "Des", typeof(VirtualDesktopHelper));
    public WidgetMainfest Info => info;
}