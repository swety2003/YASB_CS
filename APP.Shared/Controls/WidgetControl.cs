using System;
using Avalonia.Controls;
using Microsoft.Extensions.Logging;
using TB.Shared.Utils;

namespace APP.Shared.Controls;

public class WidgetControl<T> : UserControl where T : ViewModelBase, new()
{
    public WidgetControl()
    {
        VM = new T();
        VM.SetView(this);
        DataContext = VM;
    }

    public T? VM { get; }
}

// public class WidgetControl : UserControl
// {
//     public WidgetControl()
//     {
//         throw new NotSupportedException();
//     }
// }