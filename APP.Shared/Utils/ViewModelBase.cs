using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TB.Shared.Utils
{
    public abstract class ViewModelBase : ObservableObject
    {
        public ViewModelBase(UserControl control)
        {
            view = control;
        }
        public ViewModelBase()
        {
            Init();

        }
        private int interval = 1;

        public int UpdateInterval
        {
            get { return interval; }
            set
            {
                interval = value;
                if (timer != null)
                {
                    timer.Interval = new TimeSpan(0, 0, value);
                }
            }
        }

        public abstract void Update();

        public virtual void Init()
        {
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, interval);
            timer.Tick += (object? s, EventArgs e) =>
            {
                Update();
            };
            timer.Start();
        }

        private DispatcherTimer? timer { get; set; }


        private UserControl? view { get; set; }


        public T GetView<T>()
        {
            if (view is T v)
                return v;
            throw new NotSupportedException();
        }

        public void SetView(UserControl uc)
        {
            view = uc;
        }
    }
}
