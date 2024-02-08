using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using WindowsDesktop;

namespace TestPlugin.ViewModels;

public enum VDItemType
{
    Normal,AddBtn
}

public record VDItem(string name, VDItemType type, VirtualDesktop desktop);
public partial class VirtualDesktopManagerViewModel:ViewModelBase
{
    private List<VirtualDesktop> desktops;
    
    [ObservableProperty] private List<VDItem> _virtualDesktops;
    
    [ObservableProperty] private VirtualDesktop _focusedDesktop;
    
    public VirtualDesktopManagerViewModel(UserControl control) : base(control)
    {
    }
    
    private void VirtualDesktopOnCreated(object? sender, VirtualDesktop e)
    {
        e.Switch();
    }
    
    private void VirtualDesktopOnCurrentChanged(object? sender, VirtualDesktopChangedEventArgs e)
    {
        // VirtualDesktops = VirtualDesktop.GetDesktops().ToList();
        FocusedDesktop = e.NewDesktop;
    }

    // private Dictionary<VirtualDesktop, string> VDNameMap = new();
    
    internal override void OnEnabled()
    {
        Task.Run(async () =>
        {
            VirtualDesktop.Configure();
            desktops = VirtualDesktop.GetDesktops().ToList();
            VirtualDesktops = desktops.Select(x=>
                new VDItem(desktops.IndexOf(x).ToString(),
                    VDItemType.Normal,x
                )).ToList();
            VirtualDesktops.Add(new VDItem("+",VDItemType.AddBtn,null));
        
            VirtualDesktop.CurrentChanged += VirtualDesktopOnCurrentChanged;
            VirtualDesktop.Created += VirtualDesktopOnCreated;
        });

    }
    
    internal override void OnDisabled()
    {
        VirtualDesktop.CurrentChanged -= VirtualDesktopOnCurrentChanged;
        VirtualDesktop.Created -= VirtualDesktopOnCreated;
    }
}