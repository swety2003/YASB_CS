using System;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TestPlugin.ViewModels;

public abstract class ViewModelBase : ObservableObject
{
    public ViewModelBase(UserControl control)
    {
        view = control;
    }

    public DispatcherTimer? _Timer { get; set; }


    private UserControl view { get; }

    //public event EventHandler<bool>? OnActiveChanged;

    internal abstract void OnEnabled();

    internal abstract void OnDisabled();

    public T GetView<T>()
    {
        if (view is T v)
            return v;
        throw new NotSupportedException();
    }
}