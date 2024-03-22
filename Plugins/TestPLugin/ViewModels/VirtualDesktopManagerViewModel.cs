using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TB.Shared.Utils;
using TestPlugin.Extra;
using WindowsDesktop;

namespace TestPlugin.ViewModels;


public partial class VirtualDesktopManagerViewModel : ViewModelBase
{
    [ObservableProperty] private VirtualDesktop _focusedDesktop;

    [ObservableProperty] private int _focusedIndex;

    [ObservableProperty] private ObservableCollection<VirtualDesktop> _virtualDesktops;

    private List<VirtualDesktop> desktops;

    public override void Init()
    {
        base.Init();

        OnEnabled();
    }

    public override void Update()
    {
        //UpdateVD();
    }


    private void VirtualDesktopOnCreated(object? sender, VirtualDesktop e)
    {
        //UpdateVD();
        Dispatcher.UIThread.Invoke(() => { VirtualDesktops.Add(e); });
        e.Switch();
    }

    private void VirtualDesktopOnCurrentChanged(object? sender, VirtualDesktopChangedEventArgs e)
    {
        FocusedDesktop = e.NewDesktop;
    }

    [RelayCommand]
    public void NewVD()
    {
        VirtualDesktop.Create();
    }


    internal void OnEnabled()
    {
        UpdateVD();
        FocusedDesktop = VirtualDesktop.Current ?? throw new Exception();
        VirtualDesktop.CurrentChanged += VirtualDesktopOnCurrentChanged;
        VirtualDesktop.Created += VirtualDesktopOnCreated;
        VirtualDesktop.Destroyed += VirtualDesktop_Destroyed;
        VirtualDesktop.Moved += VirtualDesktop_Moved;
        //});
    }

    public void OpenFZE()
    {
        KeyInputSim.OpenFancyZonesEditor();
    }

    private void VirtualDesktop_Moved(object? sender, VirtualDesktopMovedEventArgs e)
    {
        Dispatcher.UIThread.Invoke(() => { VirtualDesktops.Move(e.OldIndex, e.NewIndex); });
    }

    private void VirtualDesktop_Destroyed(object? sender, VirtualDesktopDestroyEventArgs e)
    {
        Dispatcher.UIThread.Invoke(() => { VirtualDesktops.Remove(e.Destroyed); });
    }

    private void UpdateVD()
    {
        VirtualDesktop.Configure();
        var d = VirtualDesktop.GetDesktops();
        VirtualDesktops = new ObservableCollection<VirtualDesktop>(d);
        //foreach (var item in d)
        //{
        //    if (VirtualDesktops.Contains(item))
        //    {
        //        continue;
        //    }
        //    VirtualDesktops.Add(item);
        //}

        //foreach (var item in VirtualDesktops.ToList())
        //{
        //    if (!d.Contains(item))
        //    {
        //        VirtualDesktops.Remove(item);
        //    }
        //}
        //VirtualDesktops = new ObservableCollection<VDItem>(desktops.Select(x =>
        //    new VDItem(desktops.IndexOf(x).ToString(),
        //        VDItemType.Normal, x
        //    )).ToList());
    }
}