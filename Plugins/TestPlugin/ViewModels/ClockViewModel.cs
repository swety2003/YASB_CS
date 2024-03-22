using System;
using CommunityToolkit.Mvvm.ComponentModel;
using TB.Shared.Utils;

namespace TestPlugin.ViewModels;

public partial class ClockViewModel : ViewModelBase
{
    [ObservableProperty] private DateTime _NowTime;


    [ObservableProperty] private double hourDeg;

    [ObservableProperty] private double minDeg;

    [ObservableProperty] private double secondDeg;


    public override void Update()
    {
        NowTime = DateTime.Now;

        HourDeg = NowTime.Hour * 30 + NowTime.Minute * 30 / 60 - 90;

        MinDeg = NowTime.Minute * 6 + NowTime.Second * 6 / 60 - 90;

        SecondDeg = NowTime.Second * 6 - 90;
    }
}