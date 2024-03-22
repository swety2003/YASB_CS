using System;
using Avalonia.Controls;
using TB.Shared.Utils;

namespace APP.Shared.Controls
{
    public class WidgetControl<T> : UserControl where T : ViewModelBase, new()
    {
        
        private T? viewmodel;

        public T? VM => viewmodel;

        public WidgetControl()
        {
            viewmodel = new T();
            viewmodel.SetView(this);
            DataContext = viewmodel;


            
        }


    }

    public class MyViewBase : UserControl
    {
        public MyViewBase()
        {
            throw new NotSupportedException();
        }
    }
}
