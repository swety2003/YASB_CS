using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace TestPlugin.ViewModel
{
    public abstract class ViewModelBase : ObservableObject
    {
        public DispatcherTimer? _Timer { get; set; }

        public event EventHandler<bool>? OnActiveChanged;

        private bool _active;

        public bool Active
        {
            get { return _active; }
            set
            {
                _active = value;

                OnActiveChanged?.Invoke(this, value);

            }
        }

        private UserControl view { get; set; }

        public ViewModelBase(UserControl control)
        {
            view = control;
        }

        public T GetView<T>()
        {
            if (view is T v)
            {
                return v;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

    }
}
