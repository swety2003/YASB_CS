using System;
using Windows.Win32;
using Windows.Win32.UI.Input.KeyboardAndMouse;

namespace TestPlugin.Extra;

public static class KeyInputSim
{
    const byte VK_LWIN = 0x5B; // Virtual-Key code for the Left Windows key
    const byte VK_SHIFT = 0x10;
    const byte VK_Z = 0x5a;
    const byte VK_R = 0x52; // Virtual-Key code for 'R' key
    const uint KEYEVENTF_KEYDOWN = 0x0000; // Flag to specify key press
    const uint KEYEVENTF_KEYUP = 0x0002; // Flag to specify key release

    static void Press(byte k)
    {
        PInvoke.keybd_event(k, 0, KEYEVENTF_KEYDOWN, 0);
    }
    static void Release(byte k)
    {
        PInvoke.keybd_event(k, 0, KEYBD_EVENT_FLAGS.KEYEVENTF_KEYUP, 0);
    }
    public static void OpenFancyZonesEditor()
    {
        Press(VK_LWIN);
        Press(VK_SHIFT);
        Press(VK_Z);
        
        Release(VK_LWIN);
        Release(VK_SHIFT);
        Release(VK_Z);

    }
}