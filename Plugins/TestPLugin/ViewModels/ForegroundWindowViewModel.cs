using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Accessibility;
using Windows.Win32.UI.WindowsAndMessaging;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using TB.Shared.Utils;
using TestPlugin.Extra;
using WpfLibrary1.Extra;
using static Windows.Win32.PInvoke;

namespace TestPlugin.ViewModels;

public partial class ForegroundWindowViewModel : ViewModelBase
{
    private WINEVENTPROC _hookProc;
    
    

    [ObservableProperty] private WindowItem data;
    [ObservableProperty] private IImage icon;

    public override void Init()
    {
        base.Init();
        
        

        _hookProc = WinEventProc;
        // 监听系统的前台窗口变化。
        SetWinEventHook(
            EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND,
            HMODULE.Null, _hookProc,
            0, 0,
            WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS);

        // 开启消息循环，以便 WinEventProc 能够被调用。
        if (GetMessage(out var lpMsg, default, default, default))
        {
            TranslateMessage(in lpMsg);
            DispatchMessage(in lpMsg);
        }
    }

    private string[] spc = { "–", "-" };
    // 当前前台窗口变化时，输出新的前台窗口信息。
    private void WinEventProc(HWINEVENTHOOK hWinEventHook, uint @event, HWND hwnd, int idObject, int idChild,
        uint idEventThread, uint dwmsEventTime)
    {
        var current = GetForegroundWindow();

        var w = new Win32Window(current);
        string title = w.Title;
        // foreach (var s in spc)
        // {
        //         
        //     if (title.Contains(s))
        //     {
        //         title = title.Substring(title.IndexOf(s)+1).Trim();
        //         break;
        //     }
        // }
        Data = new WindowItem(w.Handle, title);


        try
        {
            uint hicon = PInvoke.GetClassLong(hwnd, GET_CLASS_LONG_INDEX.GCL_HICON);
            if (hicon>0)
            {
                var bmp = Bitmap.FromHicon(new IntPtr(hicon));
                Icon = bmp.ConvertToAvaloniaBitmap();
            }
            else
            {
                
                GetWindowThreadProcessId(hwnd, out var processId);
                var process = Process.GetProcessById(processId);
                // 检查进程是否存在并获取其主模块（通常是可执行文件）的路径
                if (!process.HasExited)
                {
                    var file = process.MainModule.FileName;
                    if (File.Exists(file)) Icon = FileIconUtil.GetImgByFile(file);else Icon = null;;
                }
                else Icon = null;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            
            Icon = null;
        }
    }


    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    public override void Update()
    {
    }

    public record WindowItem(IntPtr handle, string title);
}