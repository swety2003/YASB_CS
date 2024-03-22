using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.Graphics.Gdi;
using static APP.Common.NativeMethods;


namespace APP.Common;

public sealed class MonitorInfo : IEquatable<MonitorInfo>
{
    internal MonitorInfo(MONITORINFO mex)
    {
        ViewportBounds = mex.rcMonitor;
        WorkAreaBounds = mex.rcWork;
        IsPrimary = mex.dwFlags == (uint)MONITORINFOF.PRIMARY;
        //this.DeviceId = mex.;
    }

    internal RECT ViewportBounds { get; }

    internal RECT WorkAreaBounds { get; }

    internal bool IsPrimary { get; }

    internal string DeviceId { get; }

    public bool Equals(MonitorInfo other)
    {
        return DeviceId == other?.DeviceId;
    }

    internal static unsafe IEnumerable<MonitorInfo> GetAllMonitors()
    {
        var monitors = new List<MonitorInfo>();
        MONITORENUMPROC callback = delegate(HMONITOR hMonitor, HDC param1, RECT* param2, LPARAM param3)
        {
            var mi = new MONITORINFO();
            mi.cbSize = (uint)Marshal.SizeOf(typeof(MONITORINFO));
            if (!PInvoke.GetMonitorInfo(hMonitor, ref mi)) throw new Win32Exception();

            monitors.Add(new MonitorInfo(mi));
            return true;
        };

        PInvoke.EnumDisplayMonitors((HDC)0, (RECT*)0, callback, 0);

        return monitors;
    }

    public override string ToString()
    {
        return DeviceId;
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as MonitorInfo ?? throw new Exception());
    }

    public override int GetHashCode()
    {
        return DeviceId.GetHashCode();
    }

    public static bool operator ==(MonitorInfo a, MonitorInfo b)
    {
        if (ReferenceEquals(a, b)) return true;

        if (ReferenceEquals(a, null)) return false;

        return a.Equals(b);
    }

    public static bool operator !=(MonitorInfo a, MonitorInfo b)
    {
        return !(a == b);
    }
}