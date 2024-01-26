using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Shell;
using Avalonia;
using Avalonia.Controls;
using Windows.Win32;
namespace YASB.Common.WindowHelper;

internal class DesktopAppBar
{
    public enum AppBarDockMode : uint
    {
        Left = 0,
        Top,
        Right,
        Bottom
    }

    private static readonly uint AppBarMessageId = PInvoke.RegisterWindowMessage("AppBarMessage_EEDFB5206FC4");
    private readonly Window window;

    private int _DockedWidthOrHeight = 42;

    private AppBarDockMode _DockMode = AppBarDockMode.Top;

    private MonitorInfo? _Monitor;


    private bool IsAppBarRegistered;
    private bool IsInAppBarResize;
    private bool IsMinimized;

    public DesktopAppBar(Window w)
    {
        window = w;
    }

    public AppBarDockMode DockMode
    {
        get => _DockMode;
        set
        {
            _DockMode = value;
            OnDockLocationChanged();
        }
    }

    public MonitorInfo? Monitor
    {
        get => _Monitor;
        set
        {
            _Monitor = value;
            OnDockLocationChanged();
        }
    }

    public int DockedWidthOrHeight
    {
        get => _DockedWidthOrHeight;
        set
        {
            _DockedWidthOrHeight = DockedWidthOrHeight_Coerce(value);
            OnDockLocationChanged();
        }
    }

    private int DockedWidthOrHeight_Coerce(int newValue)
    {
        switch (DockMode)
        {
            case AppBarDockMode.Left:
            case AppBarDockMode.Right:
                return Clamp(newValue, (int)window.MinWidth, (int)window.MaxWidth);

            case AppBarDockMode.Top:
            case AppBarDockMode.Bottom:
                return Clamp(newValue, (int)window.MinHeight, (int)window.MaxHeight);

            default: throw new NotSupportedException();
        }
    }

    private static int Clamp(int value, int min, int max)
    {
        if (min > value) return min;
        if (max < value) return max;

        return value;
    }

    public void SetAsAppBar()
    {
        if (Design.IsDesignMode) return;

        window.SystemDecorations = SystemDecorations.None;

        var Handle = new HWND(window.TryGetPlatformHandle().Handle);


        var abd = GetAppBarData();
        PInvoke.SHAppBarMessage((uint)NativeMethods.ABM.NEW, ref abd);

        // set our initial location
        IsAppBarRegistered = true;
        OnDockLocationChanged();

        AvaloniaWinProcHelper.OnWndProcHook = MyCustomWindowProc;

        AvaloniaWinProcHelper.Init(Handle);
    }


    private int WpfDimensionToDesktop(int dim)
    {
        return dim;
    }

    private void OnDockLocationChanged()
    {
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime) return;
        if (!IsAppBarRegistered || IsInAppBarResize) return;

        var abd = GetAppBarData();
        abd.rc = GetSelectedMonitor().ViewportBounds;

        PInvoke.SHAppBarMessage((uint)NativeMethods.ABM.QUERYPOS, ref abd);

        var dockedWidthOrHeightInDesktopPixels = IsMinimized ? 0 : WpfDimensionToDesktop(DockedWidthOrHeight);

        switch (DockMode)
        {
            case AppBarDockMode.Top:
                abd.rc.bottom = abd.rc.top + dockedWidthOrHeightInDesktopPixels;
                break;
            case AppBarDockMode.Bottom:
                abd.rc.top = abd.rc.bottom - dockedWidthOrHeightInDesktopPixels;
                break;
            case AppBarDockMode.Left:
                abd.rc.right = abd.rc.left + dockedWidthOrHeightInDesktopPixels;
                break;
            case AppBarDockMode.Right:
                abd.rc.left = abd.rc.right - dockedWidthOrHeightInDesktopPixels;
                break;
            default: throw new NotSupportedException();
        }

        PInvoke.SHAppBarMessage((uint)NativeMethods.ABM.SETPOS, ref abd);

        if (!IsMinimized)
        {
            IsInAppBarResize = true;
            try
            {
                window.Position = new PixelPoint(abd.rc.left, abd.rc.top);
                window.Width = abd.rc.Width;
                window.Height = abd.rc.Height;
            }
            finally
            {
                IsInAppBarResize = false;
            }
        }
    }

    private MonitorInfo GetSelectedMonitor()
    {
        var monitor = Monitor;
        var allMonitors = MonitorInfo.GetAllMonitors();
        if (monitor == null || !allMonitors.Contains(monitor)) monitor = allMonitors.First(f => f.IsPrimary);

        return monitor;
    }

    private APPBARDATA GetAppBarData()
    {
        var Handle = new HWND(window.TryGetPlatformHandle().Handle);
        return new APPBARDATA
        {
            cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA)),
            hWnd = Handle,
            uCallbackMessage = AppBarMessageId,
            uEdge = (uint)DockMode
        };
    }


    private void MyCustomWindowProc(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam)
    {
        if (msg == NativeMethods.WM_SIZE)
        {
            IsMinimized = window.ShowInTaskbar && wParam == NativeMethods.SIZE_MINIMIZED;
            OnDockLocationChanged();
        }
        //else if (msg == WM_WINDOWPOSCHANGING && !IsInAppBarResize)
        //{
        //    var wp = Marshal.PtrToStructure<WINDOWPOS>(lParam);
        //    const int NOMOVE_NORESIZE = SWP_NOMOVE | SWP_NOSIZE;
        //    if ((wp.flags & NOMOVE_NORESIZE) != NOMOVE_NORESIZE
        //        && !IsMinimized
        //        && !(wp.x == -32_000 && wp.y == -32_000) /* loc for minimized windows */)
        //    {
        //        wp.flags |= NOMOVE_NORESIZE;
        //        Marshal.StructureToPtr(wp, lParam, false);
        //    }
        //}
        else if (msg == NativeMethods.WM_ACTIVATE)
        {
            var abd = GetAppBarData();
            PInvoke.SHAppBarMessage((uint)NativeMethods.ABM.ACTIVATE, ref abd);
        }
        else if (msg == NativeMethods.WM_WINDOWPOSCHANGED)
        {
            var abd = GetAppBarData();
            PInvoke.SHAppBarMessage((uint)NativeMethods.ABM.WINDOWPOSCHANGED, ref abd);
        }
        else if (msg == AppBarMessageId)
        {
            switch ((NativeMethods.ABN)wParam.Value)
            {
                case NativeMethods.ABN.POSCHANGED:
                    OnDockLocationChanged();
                    break;
            }
        }
        //return IntPtr.Zero;
    }

    public void UnSetAppBar()
    {
        if (IsAppBarRegistered)
        {
            var abd = GetAppBarData();
            PInvoke.SHAppBarMessage((uint)NativeMethods.ABM.REMOVE, ref abd);
            IsAppBarRegistered = false;
        }
    }
}