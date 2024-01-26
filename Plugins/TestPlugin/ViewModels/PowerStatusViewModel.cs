using System;
using Windows.Win32;
using Windows.Win32.System.Power;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TestPlugin.ViewModels;

public partial class PowerStatusViewModel:ViewModelBase
{
    public PowerStatusViewModel(UserControl control) : base(control)
    {
        _Timer = new DispatcherTimer();
        _Timer.Interval = new TimeSpan(0, 0, 1);
    }

    internal override void OnEnabled()
    {
        _Timer.Tick += TimerOnTick;
        _Timer.Start();
    }

    private readonly string[] onlineIcon = {
        "\uf0a2",
        "\uf0a2",
        "\uf0a5",
        "\uf0a6",
        "\uf0a7",
        "\uf0a7",
        "\ue1a3"
    };
    private readonly string[] offlineIcon = {
        "\ue19c",
        "\uebd9",
        "\uebe0",
        "\uebe2",
        "\uebd4",
        "\uebd2",
        "\uebd2"
    };
    private char ParseUnicodeEscapedCharacter(string escapedSequence)
    {
        if (escapedSequence.StartsWith(@"&#") && escapedSequence.EndsWith(";"))
        {
            int codePoint;
            if (int.TryParse(escapedSequence.Substring(2, escapedSequence.Length - 3), out codePoint))
            {
                return (char)codePoint;
            }
        }

        return '\0'; // 返回默认字符或抛出异常，根据实际情况选择
    }
    private void TimerOnTick(object? sender, EventArgs e)
    {
        SYSTEM_POWER_STATUS status;
        PInvoke.GetSystemPowerStatus(out status);
        LifePercent = status.BatteryLifePercent;
        switch (status.ACLineStatus)
        {
            case 0:
            {   // offline
                int index = (int)Math.Ceiling((decimal)(status.BatteryLifePercent / 15));
                Icon = offlineIcon[index];

            } break;
            case 1:
            {
                //online
                int index = (int)Math.Ceiling((decimal)(status.BatteryLifePercent / 15));
                Icon = onlineIcon[index];
            } break;
            case 255:
            {
                Icon = "&#xe1a6;";
            } break;
            default:
            {
                Icon = "&#xe1a6;";

            } break;
        }

    }

    internal override void OnDisabled()
    {
        _Timer.Tick -= TimerOnTick;
        _Timer.Stop();
    }

    [ObservableProperty] private int lifePercent;

    [ObservableProperty] private string icon;
}