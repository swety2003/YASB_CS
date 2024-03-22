// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Windows.Win32.Foundation;
using Windows.Win32.System.Memory;
using Windows.Win32.System.Threading;
using static Windows.Win32.PInvoke;
using Bitmap = Avalonia.Media.Imaging.Bitmap;
using S_Bitmap = System.Drawing.Bitmap;

#pragma warning disable CA1416
namespace TestPlugin.Extra;

public static class TrayIconHelper
{
    private static HWND FindTrayWnd()
    {
        var hWnd = HWND.Null;

        hWnd = FindWindow("Shell_TrayWnd", null);
        hWnd = FindWindowEx(hWnd, HWND.Null, "TrayNotifyWnd", null);
        hWnd = FindWindowEx(hWnd, HWND.Null, "SysPager", null);
        hWnd = FindWindowEx(hWnd, HWND.Null, "ToolbarWindow32", null);

        return hWnd;
    }

    private static unsafe void R_CLICK(HWND hWnd, HANDLE hProcess, int i)
    {
        var rect = new RECT();
        var tray_rect = VirtualAllocEx(hProcess, (void*)0, (nuint)sizeof(RECT), VIRTUAL_ALLOCATION_TYPE.MEM_COMMIT,
            PAGE_PROTECTION_FLAGS.PAGE_READWRITE);


        SendMessage(hWnd, TB_GETITEMRECT, new WPARAM((nuint)i), new LPARAM((nint)tray_rect));

        ReadProcessMemory(hProcess, tray_rect, &rect, (nuint)sizeof(RECT)); //读取托盘区域数据

        SendMessage(hWnd, TB_SETHOTITEM, new WPARAM((nuint)i), 0);

        SendMessage(hWnd, WM_RBUTTONDOWN, 1, MAKELPARAM((ushort)(rect.left + 2), (ushort)(rect.top + 2)));
        SendMessage(hWnd, WM_RBUTTONUP, 0, MAKELPARAM((ushort)(rect.left + 2), (ushort)(rect.top + 2)));

        VirtualFreeEx(hProcess, tray_rect, (nuint)sizeof(RECT), VIRTUAL_FREE_TYPE.MEM_RELEASE);
    }

    internal static unsafe TrayItemData[] EnumNotifyWindow()
    {
        var hTrayWnd = FindTrayWnd();

        var stlTrayItems = new SortedList<string, TrayItemData>();


        uint dwProcessId = 0;
        var tbButtonInfo = new TBBUTTON();
        var trayData = new TRAYDATA();

        GetWindowThreadProcessId(hTrayWnd, &dwProcessId);

        var hTrayProcess = OpenProcess(
            PROCESS_ACCESS_RIGHTS.PROCESS_VM_OPERATION | // 需要在进程的地址空间上执行操作
            PROCESS_ACCESS_RIGHTS.PROCESS_VM_READ | // 需要使用 ReadProcessMemory 读取进程中的内存
            PROCESS_ACCESS_RIGHTS.PROCESS_VM_WRITE, // 需要在使用 WriteProcessMemory 的进程中写入内存
            false, // 子进程不继承句柄
            dwProcessId // 目标进程 PID
        );
        // 在进程虚拟空间中分配内存，用来接收 TBBUTTON 结构体指针
        var iAllocBaseAddress = VirtualAllocEx(
            hTrayProcess, // 目标进程句柄
            (void*)0, // 内存起始地址（默认）
            (nuint)sizeof(TBBUTTON), // 内存大小
            VIRTUAL_ALLOCATION_TYPE.MEM_COMMIT, // 内存类型（提交）
            PAGE_PROTECTION_FLAGS.PAGE_EXECUTE_READWRITE // 内存保护属性（可读可写可执行）
        );
        var iTrayItemCount = (nuint)SendMessage(hTrayWnd, TB_BUTTONCOUNT, 0, 0).Value;

        for (nuint i = 0; i < iTrayItemCount; i++)
        {
            var hButtonInfo = Marshal.AllocHGlobal(Marshal.SizeOf(tbButtonInfo));
            var hTrayData = Marshal.AllocHGlobal(Marshal.SizeOf(trayData));

            nuint* iOut = null;

            SendMessage(hTrayWnd, TB_GETBUTTON, new WPARAM(i), new LPARAM((nint)iAllocBaseAddress));

            ReadProcessMemory(hTrayProcess, iAllocBaseAddress, (void*)hButtonInfo, (nuint)Marshal.SizeOf(tbButtonInfo),
                iOut);
            ReadProcessMemory(hTrayProcess, (void*)tbButtonInfo.dwData, (void*)hTrayData,
                (nuint)Marshal.SizeOf(trayData), iOut);

            Marshal.PtrToStructure(hButtonInfo, tbButtonInfo);
            Marshal.PtrToStructure(hTrayData, trayData);

            Marshal.FreeHGlobal(hButtonInfo);
            Marshal.FreeHGlobal(hTrayData);

            var bytTextData = new char[1024];

            var i_data_offset = 12;
            var i_str_offset = 18;
            // 判断 x64
            if (Environment.Is64BitOperatingSystem)
            {
                i_data_offset += 4;
                i_str_offset += 6;
            }

            string ws_filePath;
            string strTrayToolTip;
            fixed (char* hbytTextData = bytTextData) // char* windowNameChars = stackalloc char[bufferSize];
            {
                ReadProcessMemory(hTrayProcess, (void*)tbButtonInfo.iString, hbytTextData, 1024, iOut);
                strTrayToolTip = new string(hbytTextData);
                // ws_filePath = new string( hbytTextData + i_str_offset);
                // strTrayToolTip = new string( hbytTextData + i_str_offset + MAX_PATH);
            }

            var trayItem = new TrayItemData();
            //trayItem.dwProcessID = (int)dwProcessID;
            trayItem.fsState = tbButtonInfo.fsState;
            trayItem.fsStyle = tbButtonInfo.fsStyle;
            try
            {
                trayItem.ico = S_Bitmap.FromHicon(trayData.hIcon).ConvertToAvaloniaBitmap();
            }
            catch (Exception e)
            {
            }

            //trayItem.hProcess = hRelProcess;
            trayItem.hWnd = trayData.hwnd;
            // trayItem.idBitmap = tbButtonInfo.iBitmap;
            // trayItem.idCommand = tbButtonInfo.idCommand;
            //trayItem.lpProcImagePath = strImageFilePath;
            trayItem.lpTrayToolTip = strTrayToolTip;
            trayItem.index = i;

            stlTrayItems[string.Format("{0:d8}", tbButtonInfo.idCommand)] = trayItem;
        }

        VirtualFreeEx(hTrayProcess, iAllocBaseAddress, (nuint)Marshal.SizeOf(tbButtonInfo),
            VIRTUAL_FREE_TYPE.MEM_RELEASE);
        CloseHandle(hTrayProcess);


        var trayItems = new TrayItemData[stlTrayItems.Count];
        stlTrayItems.Values.CopyTo(trayItems, 0);
        return trayItems;
    }


    public record TrayItemData
    {
        public int dwProcessID;
        public byte fsState;
        public byte fsStyle;
        public IntPtr hProcess;
        public IntPtr hWnd;
        public int idBitmap;
        public int idCommand;

        public nuint index;
        public string lpProcImagePath;
        public Bitmap ico { get; set; }
        public string lpTrayToolTip { get; set; }
    }


    [StructLayout(LayoutKind.Sequential)]
    internal class TBBUTTON
    {
        public byte bReserved1;
        public byte bReserved2;
        public byte bReserved3;
        public byte bReserved4;
        public byte bReserved5;
        public byte bReserved6;

        public UIntPtr dwData;
        public byte fsState;
        public byte fsStyle;
        public int iBitmap;
        public int idCommand;
        public IntPtr iString;
    }

    [StructLayout(LayoutKind.Sequential)]
    public class TRAYDATA
    {
        public IntPtr hIcon; //托盘图标的句柄 
        public IntPtr hwnd;
        public int Reserved0;
        public int Reserved1;
        public uint uCallbackMessage;
        public uint uID;
    }
}