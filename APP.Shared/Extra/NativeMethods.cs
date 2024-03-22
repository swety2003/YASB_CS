using System;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Shell;

namespace APP.Common;

internal static class NativeMethods
{
    
    [DllImport("Dwmapi.dll")]
    public static extern int DwmGetWindowAttribute(nint hWnd, uint dwAttribute, out RECT pvAttribute, int cbAttribute);
    [DllImport("Shell32.dll", ExactSpelling = true)]
    public static extern uint SHAppBarMessage(ABM dwMessage, ref APPBARDATA pData);
    public struct WINDOWPOS
    {
        public nint hwnd;

        public nint hwndInsertAfter;

        public int x;

        public int y;

        public int cx;

        public int cy;

        public int flags;

        public RECT Bounds
        {
            get
            {
                return new RECT(x, y, cx, cy);
            }
            set
            {
                x = (int)value.X;
                y = (int)value.Y;
                cx = (int)value.Width;
                cy = (int)value.Height;
            }
        }
    }

    [Flags]
    public enum ABM
    {
        NEW = 0,
        REMOVE,
        QUERYPOS,
        SETPOS,
        GETSTATE,
        GETTASKBARPOS,
        ACTIVATE,
        GETAUTOHIDEBAR,
        SETAUTOHIDEBAR,
        WINDOWPOSCHANGED,
        SETSTATE
    }
    [Flags]
    public enum ABN
    {
        STATECHANGE = 0,
        POSCHANGED,
        FULLSCREENAPP,
        WINDOWARRANGE
    }

    [Flags]
    public enum MONITOR_DPI_TYPE
    {
        MDT_EFFECTIVE_DPI = 0,
        MDT_ANGULAR_DPI = 1,
        MDT_RAW_DPI = 2,
        MDT_DEFAULT = MDT_EFFECTIVE_DPI
    }

    [Flags]
    public enum MONITORINFOF
    {
        PRIMARY = 0x1
    }

    public const int
        SWP_NOMOVE = 0x0002,
        SWP_NOSIZE = 0x0001;

    public const int
        WM_SIZE = 0x0005,
        WM_ACTIVATE = 0x0006,
        WM_WINDOWPOSCHANGED = 0x0047,
        WM_SYSCOMMAND = 0x0112,
        WM_WINDOWPOSCHANGING = 0x0046;

    public const int
        SC_MOVE = 0xF010;

    public const int
        SIZE_MINIMIZED = 1;

    public const int
        DWMWA_EXTENDED_FRAME_BOUNDS = 9;

    public const int
        GWL_EXSTYLE = -20;

    public const int
        WS_EX_TOOLWINDOW = 0x00000080;

    private const int CCHDEVICENAME = 32;

    public static int GWLP_WNDPROC = -4;
}