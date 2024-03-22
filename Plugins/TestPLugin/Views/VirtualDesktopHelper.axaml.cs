using System;
using System.IO;
using System.Reflection;
using APP.Shared;
using APP.Shared.Controls;
using TestPlugin.ViewModels;
using WindowsDesktop;

namespace TestPlugin.Views;

[Widget("虚拟桌面控制", "23333")]
public partial class VirtualDesktopHelper : WidgetControl<VirtualDesktopManagerViewModel>
{
    public VirtualDesktopHelper()
    {
        InitializeComponent();
    }

    static VirtualDesktopHelper()
    {
        AppDomain.CurrentDomain.AssemblyResolve += (_, args) =>
            args.Name.StartsWith("VirtualDesktop") ? Assembly.GetAssembly(typeof(VirtualDesktop)) : null;
    }

}