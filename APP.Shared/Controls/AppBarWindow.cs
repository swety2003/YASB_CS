using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media;
using Windows.Win32;
using APP.Common;
using APP.Common.WindowHelper;
using Avalonia;
using Avalonia.Controls;
using Rect = System.Windows.Rect;
using Thickness = System.Windows.Thickness;
using Window = Avalonia.Controls.Window;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Shell;
using Windows.Win32.UI.WindowsAndMessaging;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Point = Avalonia.Point;
using Size = Avalonia.Size;

namespace APP.Controls;

public enum AppBarDockMode
{
    Left,
    Top,
    Right,
    Bottom
}
public class AppBarWindow:Window
{
    private bool IsAppBarRegistered;

    private bool IsInAppBarResize;

    private bool IsMinimized;

    public static readonly StyledProperty<AppBarDockMode> DockModeProperty = AvaloniaProperty.Register<AppBarWindow, AppBarDockMode>(nameof(DockMode), defaultValue: AppBarDockMode.Top,coerce:DockLocation_Changed);

    public static readonly StyledProperty<MonitorInfo> MonitorProperty =  AvaloniaProperty.Register<AppBarWindow, MonitorInfo>(nameof(DockMode), defaultValue: null,coerce:DockLocation_Changed);

    private static MonitorInfo DockLocation_Changed(AvaloniaObject d, MonitorInfo arg2)
    {
        ((AppBarWindow)d).OnDockLocationChanged();
        return arg2;
    }

    public static readonly StyledProperty<int> DockedWidthOrHeightProperty =  AvaloniaProperty.Register<AppBarWindow, int>(nameof(DockMode), defaultValue: 40,coerce:DockedWidthOrHeight_Coerce);


    private static uint _AppBarMessageId;

    private Thickness FrameThickness;

    private PixelPoint LastPosition = new(-32000, -32000);

    public AppBarDockMode DockMode
    {
        get => GetValue(DockModeProperty);
        set => SetValue(DockModeProperty, value);
    }

    public MonitorInfo Monitor
    {
        get
        {
            return GetValue(MonitorProperty);
        }
        set => SetValue(MonitorProperty, value);
    }

    public int DockedWidthOrHeight
    {
        get => GetValue(DockedWidthOrHeightProperty);
        set => SetValue(DockedWidthOrHeightProperty, value);
    }

    public static uint AppBarMessageId
    {
        get
        {
            if (_AppBarMessageId == 0)
            {
                _AppBarMessageId = PInvoke.RegisterWindowMessage("AppBarMessage_YASB_SC");
            }

            return _AppBarMessageId;
        }
    }

    private RECT WindowBounds
    {
        set
        {
            var TopLeft = new PixelPoint(value.top,value.left);
            Thickness ft = FrameThickness;
            if (LastPosition != TopLeft)
            {
                FrameThickness = (ft = default);
            }

            SetTopLeft();
            if (LastPosition != TopLeft)
            {
                FrameThickness = (ft = GetFrameThickness());
                LastPosition = TopLeft;
            }

            if (FrameThickness.Top != 0.0 || FrameThickness.Left != 0.0)
            {
                SetTopLeft();
            }


            this.ClientSize = new Size(Width,Height);
            
            void SetTopLeft()
            {
                Position = new PixelPoint(DesktopDimensionToWpf(value.left - ft.Left),
                    DesktopDimensionToWpf(value.top - ft.Top));
            }
        }
    }

    static AppBarWindow()
    {
        ShowInTaskbarProperty.OverrideMetadata(typeof(AppBarWindow), new StyledPropertyMetadata<bool>(false));
        MinHeightProperty.OverrideMetadata(typeof(AppBarWindow), new StyledPropertyMetadata<double>(20.0,coerce: MinMaxHeightWidth_Changed));
        MinWidthProperty.OverrideMetadata(typeof(AppBarWindow), new StyledPropertyMetadata<double>(20.0, coerce:MinMaxHeightWidth_Changed));
        MaxHeightProperty.OverrideMetadata(typeof(AppBarWindow), new StyledPropertyMetadata<double>(coerce:MinMaxHeightWidth_Changed));
        MaxWidthProperty.OverrideMetadata(typeof(AppBarWindow), new StyledPropertyMetadata<double>(coerce:MinMaxHeightWidth_Changed));
    }

    public AppBarWindow()
    {
        SystemDecorations = SystemDecorations.None;
        // CanResize = false;
        Topmost = true;
    }

    private static int DockedWidthOrHeight_Coerce(AvaloniaObject d, int value)
    {
        AppBarWindow appBarWindow = (AppBarWindow)d;
        switch (appBarWindow.DockMode)
        {
            case AppBarDockMode.Left:
            case AppBarDockMode.Right:
                return BoundIntToDouble(value, appBarWindow.MinWidth, appBarWindow.MaxWidth);
            case AppBarDockMode.Top:
            case AppBarDockMode.Bottom:
                return BoundIntToDouble(value, appBarWindow.MinHeight, appBarWindow.MaxHeight);
            default:
                throw new NotSupportedException();
        }
    }

    private static int BoundIntToDouble(int value, double min, double max)
    {
        if (min > value)
        {
            return (int)Math.Ceiling(min);
        }

        if (max < value)
        {
            return (int)Math.Floor(max);
        }

        return value;
    }

    private static double MinMaxHeightWidth_Changed(AvaloniaObject d, double e)
    {
        d.CoerceValue(DockedWidthOrHeightProperty);

        return e;
    }

    private static AppBarDockMode DockLocation_Changed(AvaloniaObject d, AppBarDockMode appBarDockMode)
    {
        ((AppBarWindow)d).OnDockLocationChanged();
        return appBarDockMode;
    }

    // protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
    // {
    //     base.OnDpiChanged(oldDpi, newDpi);
    //     OnDockLocationChanged();
    // }

    private unsafe delegate* unmanaged<HWND, uint, WPARAM, LPARAM, LRESULT> _wrappedAvaloniaWinProc;
    private delegate LRESULT WndProc(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam);
    private WndProc _proc;
    private HWND desktopHandle;
    private HWND shellHandle;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        
        desktopHandle = PInvoke.GetDesktopWindow();
        shellHandle = PInvoke.GetShellWindow();
        unsafe
        {
            base.OnApplyTemplate(e);
            var handle = new HWND(TryGetPlatformHandle().Handle);

            if (!ShowInTaskbar)
            {
                ulong num = (ulong)PInvoke.GetWindowLongPtr(handle, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE);
                num |= 0x80;
                PInvoke.SetWindowLongPtr(handle, WINDOW_LONG_PTR_INDEX.GWL_EXSTYLE, (nint)num);
            }
        
            _wrappedAvaloniaWinProc =
                (delegate* unmanaged<HWND, uint, WPARAM, LPARAM, LRESULT>)PInvoke.GetWindowLongPtr(handle,
                    WINDOW_LONG_PTR_INDEX.GWLP_WNDPROC);
        
            _proc = MyCustomWindowProc;
        
            PInvoke.SetWindowLongPtr(handle, WINDOW_LONG_PTR_INDEX.GWLP_WNDPROC,Marshal.GetFunctionPointerForDelegate(_proc) );

        
            APPBARDATA pData = GetAppBarData();
            NativeMethods.SHAppBarMessage(NativeMethods.ABM.NEW, ref pData);
            IsAppBarRegistered = true;
            OnDockLocationChanged();
        }
    }

    protected override void OnClosing(WindowClosingEventArgs e)
    {
        base.OnClosing(e);
        if (!e.Cancel && IsAppBarRegistered)
        {
            APPBARDATA pData = GetAppBarData();
            NativeMethods.SHAppBarMessage(NativeMethods.ABM.REMOVE, ref pData);
            IsAppBarRegistered = false;
        }
    }

    private double GetScale()
    {
        using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
        {
            float dpiX = graphics.DpiX;
            float dpiY = graphics.DpiY;
            // 计算缩放倍率  
            // 通常认为96 DPI是标准DPI，所以缩放倍率 = 当前DPI / 96  
            double scaleX = dpiX / 96.0;  
            return scaleX;
        }
    }
    private int WpfDimensionToDesktop(double dim)
    {
        return (int)Math.Ceiling(dim * GetScale());
    }

    private int DesktopDimensionToWpf(double dim)
    {
        return (int)(dim / GetScale());
    }

    private void OnDockLocationChanged()
    {
        // if (DesignerProperties.GetIsInDesignMode(this) || !IsAppBarRegistered || IsInAppBarResize)
        // {
        //     return;
        // }

        APPBARDATA pData = GetAppBarData();
        pData.rc = GetSelectedMonitor().ViewportBounds;
        NativeMethods.SHAppBarMessage(NativeMethods.ABM.QUERYPOS, ref pData);
        int num = ((!IsMinimized) ? WpfDimensionToDesktop(DockedWidthOrHeight) : 0);
        switch (DockMode)
        {
            case AppBarDockMode.Top:
                pData.rc.bottom = pData.rc.top + num;
                break;
            case AppBarDockMode.Bottom:
                pData.rc.top = pData.rc.bottom - num;
                break;
            case AppBarDockMode.Left:
                pData.rc.right = pData.rc.left + num;
                break;
            case AppBarDockMode.Right:
                pData.rc.left = pData.rc.right - num;
                break;
            default:
                throw new NotSupportedException();
        }

        NativeMethods.SHAppBarMessage(NativeMethods.ABM.SETPOS, ref pData);
        if (IsMinimized)
        {
            return;
        }

        IsInAppBarResize = true;
        try
        {
            WindowBounds = pData.rc;
        }
        finally
        {
            IsInAppBarResize = false;
        }
    }

    private MonitorInfo GetSelectedMonitor()
    {
        MonitorInfo monitorInfo = Monitor;
        IEnumerable<MonitorInfo> allMonitors = MonitorInfo.GetAllMonitors();
        if (monitorInfo == null || !allMonitors.Contains(monitorInfo))
        {
            monitorInfo = allMonitors.First(f => f.IsPrimary);
        }

        return monitorInfo;
    }

    private APPBARDATA GetAppBarData()
    {
        var handle = new HWND(TryGetPlatformHandle().Handle);

        APPBARDATA result = default(APPBARDATA);
        result.cbSize = (uint)Marshal.SizeOf(typeof(APPBARDATA));
        result.hWnd = handle;
        result.uCallbackMessage = AppBarMessageId;
        result.uEdge = (uint)DockMode;
        return result;
    }

    private unsafe LRESULT MyCustomWindowProc(HWND hwnd, uint msg, WPARAM wParam, LPARAM lParam)
    {
        if (msg == 5)
        {
            IsMinimized = base.ShowInTaskbar && wParam == 1;
            OnDockLocationChanged();
        }
        else if (msg == 70 && !IsInAppBarResize)
        {
            NativeMethods.WINDOWPOS structure = Marshal.PtrToStructure<NativeMethods.WINDOWPOS>(lParam);
            if ((structure.flags & 3) != 3 && !IsMinimized && (structure.x != -32000 || structure.y != -32000))
            {
                structure.flags |= 3;
                Marshal.StructureToPtr(structure, lParam, fDeleteOld: false);
            }
        }
        else
        {
            switch (msg)
            {
                case 6:
                    {
                        APPBARDATA pData2 = GetAppBarData();
                        NativeMethods.SHAppBarMessage(NativeMethods.ABM.ACTIVATE, ref pData2);
                        break;
                    }
                case 71:
                    {
                        APPBARDATA pData = GetAppBarData();
                        NativeMethods.SHAppBarMessage(NativeMethods.ABM.WINDOWPOSCHANGED, ref pData);
                        break;
                    }
                default:
                    if (msg == AppBarMessageId && wParam == 1)
                    {
                        OnDockLocationChanged();
                    }

                    break;
            }
        }
        const int ABN_STATECHANGE = 0x0000000;
        const int ABN_POSCHANGED = 0x0000001;
        const int ABN_FULLSCREENAPP = 0x0000002;
        const int ABN_WINDOWARRANGE = 0x0000003; // lParam == TRUE means hide
        if (msg == AppBarMessageId)
        {
            switch (wParam.Value)
            {
                case (int)ABN_FULLSCREENAPP:
                {
                    IntPtr hWnd = PInvoke.GetForegroundWindow();
                    //判断当前全屏的应用是否是桌面
                    if (hWnd.Equals(desktopHandle) || hWnd.Equals(shellHandle))
                    {
                        RunningFullScreenApp = false;
                        break;
                    }
                    //判断是否全屏
                    if (lParam == 1)
                    {
                        this.RunningFullScreenApp = true; this.WindowState = WindowState.Minimized;
                    }
                    else
                    {
                        this.RunningFullScreenApp = false; this.WindowState = WindowState.Normal;
                    }

                    break;
                }
                default:
                    break;
            }
        }
        return _wrappedAvaloniaWinProc(hwnd, msg, wParam, lParam);
    }

    public bool RunningFullScreenApp { get; private set; }

    private Thickness GetFrameThickness()
    {
        var handle = new HWND(TryGetPlatformHandle().Handle);
        if (!PInvoke.GetWindowRect(handle, out var rect))
        {
            return default;
        }
        
        if (NativeMethods.DwmGetWindowAttribute(handle, 9u, out var pvAttribute, Marshal.SizeOf<RECT>()) != 0)
        {
            return default;
        }
        
        return new Thickness(pvAttribute.left - rect.left, pvAttribute.top - rect.top, rect.right - pvAttribute.right, rect.bottom - pvAttribute.bottom);
    }
}