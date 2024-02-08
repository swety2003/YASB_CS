using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.WindowsAndMessaging;

namespace APP.Common;

public static unsafe class AvaloniaWinProcHelper
{
    private static delegate* unmanaged<HWND, uint, WPARAM, LPARAM, LRESULT> _wrappedAvaloniaWinProc;

    internal static WndProcHookD? OnWndProcHook;

    internal static HWND WindowHandle { get; private set; }

    internal static void Init(HWND handle)
    {
        WindowHandle = handle;

        _wrappedAvaloniaWinProc =
            (delegate* unmanaged<HWND, uint, WPARAM, LPARAM, LRESULT>)PInvoke.GetWindowLong(WindowHandle,
                WINDOW_LONG_PTR_INDEX.GWLP_WNDPROC);
        delegate* unmanaged<HWND, uint, WPARAM, LPARAM, LRESULT> wrapperWinProc = &WndProcHook;
        PInvoke.SetWindowLong(WindowHandle, WINDOW_LONG_PTR_INDEX.GWLP_WNDPROC, (int)wrapperWinProc);
    }

    [UnmanagedCallersOnly]
    internal static LRESULT WndProcHook(HWND hwnd, uint code, WPARAM wParam, LPARAM lParam)
    {
        //Debug.WriteLine($"msg - {code}");

        OnWndProcHook?.Invoke(hwnd, code, wParam, lParam);
        return _wrappedAvaloniaWinProc(hwnd, code, wParam, lParam);
    }

    internal delegate void WndProcHookD(HWND hwnd, uint code, WPARAM wParam, LPARAM lParam);
}