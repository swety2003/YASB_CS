using System;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TestPlugin.ViewModels;

internal partial class ClockViewModel : ViewModelBase
{
    [ObservableProperty] private DateTime _NowTime;


    [ObservableProperty] private double hourDeg;

    [ObservableProperty] private double minDeg;

    [ObservableProperty] private double secondDeg;

    public ClockViewModel(UserControl control) : base(control)
    {
        _Timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
    }

    private void _Timer_Tick(object? sender, EventArgs e)
    {
        NowTime = DateTime.Now;

        HourDeg = _NowTime.Hour * 30 + _NowTime.Minute * 30 / 60 - 90;

        MinDeg = _NowTime.Minute * 6 + _NowTime.Second * 6 / 60 - 90;

        SecondDeg = _NowTime.Second * 6 - 90;
    }

    internal override void OnEnabled()
    {
        _Timer.Start();
        _Timer.Tick += _Timer_Tick;
    }

    internal override void OnDisabled()
    {
        _Timer.Stop();

        _Timer.Tick -= _Timer_Tick;
    }
}