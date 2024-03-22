using System;
using System.Text;
using Windows.Win32;
using Windows.Win32.System.Power;
using CommunityToolkit.Mvvm.ComponentModel;
using TB.Shared.Utils;

namespace TestPlugin.ViewModels;

public partial class PowerStatusViewModel : ViewModelBase
{
    [ObservableProperty] private string icon;

    [ObservableProperty] private int lifePercent;

    public override void Update()

    {
        TimerOnTick();
    }

    private void TimerOnTick()
    {
        SYSTEM_POWER_STATUS status;
        PInvoke.GetSystemPowerStatus(out status);
        LifePercent = status.BatteryLifePercent;
        var utf8 = Encoding.UTF8;
        switch (status.ACLineStatus)
        {
            case 0:
            {
                // offline
                var index = (int)Math.Ceiling((decimal)(status.BatteryLifePercent / 10));
                // Icon = offlineIcon[index]
                var c = '\uF5F2';
                Icon = ((char)(c + index)).ToString();
            }
                break;
            case 1:
            {
                //online
                var index = (int)Math.Ceiling((decimal)(status.BatteryLifePercent / 10));
                // Icon = onlineIcon[index];
                var c = '\uF5FD';

                Icon = ((char)(c + index)).ToString();
            }
                break;
            case 255:
            {
                Icon = "&#xe1a6;";
            }
                break;
            default:
            {
                Icon = "&#xe1a6;";
            }
                break;
        }
    }
}