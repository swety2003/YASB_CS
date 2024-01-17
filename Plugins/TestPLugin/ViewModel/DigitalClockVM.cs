using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TestPlugin.ViewModel
{
    internal partial class DigitalClockVM : ViewModelBase
    {
        public DigitalClockVM(UserControl control) : base(control)
        {
            _Timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
            this.OnActiveChanged += DigitalClockVM_OnActiveChanged;
        }

        private void DigitalClockVM_OnActiveChanged(object? sender, bool e)
        {
            if (e)
            {
                _Timer.Start();
                _Timer.Tick += _Timer_Tick;
            }
            else
            {
                _Timer.Stop();

                _Timer.Tick -= _Timer_Tick;

            }
        }

        [ObservableProperty]
        private double hourDeg = 0;

        [ObservableProperty]
        private double minDeg = 0;

        [ObservableProperty]
        private double secondDeg = 0;

        private void _Timer_Tick(object? sender, EventArgs e)
        {
            NowTime = DateTime.Now;

            HourDeg = _NowTime.Hour * 30 + _NowTime.Minute * 30 / 60 - 90;

            MinDeg = _NowTime.Minute * 6 + _NowTime.Second * 6 / 60 - 90;

            SecondDeg = _NowTime.Second * 6 - 90;

        }

        [ObservableProperty]
        DateTime _NowTime;
    }
}
