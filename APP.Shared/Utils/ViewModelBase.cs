﻿using System;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TB.Shared.Utils;

public abstract class ViewModelBase : ObservableObject
{
    private int interval = 1;

    public ViewModelBase(UserControl control)
    {
        view = control;
    }

    public ViewModelBase()
    {
        Init();
    }

    public int UpdateInterval
    {
        get => interval;
        set
        {
            interval = value;
            if (timer != null) timer.Interval = new TimeSpan(0, 0, value);
        }
    }

    private DispatcherTimer? timer { get; set; }


    private UserControl? view { get; set; }

    public abstract void Update();

    public virtual void Init()
    {
        timer = new DispatcherTimer();
        timer.Interval = new TimeSpan(0, 0, interval);
        timer.Tick += (s, e) => { Update(); };
        timer.Start();
    }


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