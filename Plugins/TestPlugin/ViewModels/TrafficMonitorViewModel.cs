using System;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CZGL.SystemInfo;

namespace TestPlugin.ViewModels;

internal partial class TrafficMonitorViewModel : ViewModelBase
{
    private readonly NetworkInfo network;
    [ObservableProperty] private int cpuLoad;

    [ObservableProperty] private NetItem download;
    private Rate oldRate;

    [ObservableProperty] private int ramLoad;

    [ObservableProperty] private NetItem upload;

    private CPUTime v1;

    public TrafficMonitorViewModel(UserControl control) : base(control)
    {
        v1 = CPUHelper.GetCPUTime();
        network = NetworkInfo.TryGetRealNetworkInfo() ?? throw new Exception();
        oldRate = network.GetIpv4Speed();
    }

    internal override void OnEnabled()
    {
        _Timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _Timer.Tick += _Timer_Tick;
        _Timer.Start();
    }

    internal override void OnDisabled()
    {
        _Timer.Tick -= _Timer_Tick;

        _Timer.Stop();
    }

    private void _Timer_Tick(object? sender, EventArgs e)
    {
        var v2 = CPUHelper.GetCPUTime();
        var value = CPUHelper.CalculateCPULoad(v1, v2);
        v1 = v2;

        var memory = MemoryHelper.GetMemoryValue();
        var newRate = network.GetIpv4Speed();
        var speed = NetworkInfo.GetSpeed(oldRate, newRate);
        oldRate = newRate;

        CpuLoad = (int)(value * 100);
        RamLoad = (int)memory.UsedPercentage;
        Upload = new NetItem(speed.Sent.Size.ToString(), speed.Sent.SizeType.ToString());
        Download = new NetItem(speed.Received.Size.ToString(), speed.Received.SizeType.ToString());
    }

    public record NetItem(string size, string type);
}