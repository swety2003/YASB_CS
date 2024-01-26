using System;
using Windows.Win32;
using Avalonia.Controls;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace YASB.Common.WindowHelper;

internal class HideInRecentTasksHelper
{
    [Flags]
    public enum ExtendedWindowStyles:int
    {
        WS_EX_TOOLWINDOW = 0x00000080
    }

    private readonly Window window;

    public HideInRecentTasksHelper(Window w)
    {
        window = w;
    }


    public void HideInRecentTasks()
    {
        if (Design.IsDesignMode) return;
        if (window.ShowInTaskbar) window.ShowInTaskbar = false;
        var handle = new HWND(window.TryGetPlatformHandle().Handle);

        var exStyle = PInvoke.GetWindowLong(handle, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
        exStyle |=(int)ExtendedWindowStyles.WS_EX_TOOLWINDOW;
        PInvoke.SetWindowLong(handle, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, exStyle);
    }
}