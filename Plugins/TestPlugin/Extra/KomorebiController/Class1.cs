﻿using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

namespace TestPlugin.Extra;

public static class CommandHelper
{
    public static void StartProcess()
    {
        var config_file = Path.Combine(Plugin.GetRunningDictionary(), "Vendor", "komorebi", "data", "komorebi.json");
        CallKomorebic($"start -c {config_file} --whkd");
    }

    public static bool Running()
    {
        var p = Process.GetProcessesByName("komorebi");
        return p.Length > 0;
    }

    public static void SetWinHideBehavior()
    {
        CallKomorebic("window-hiding-behaviour hide");
    }

    public static void StopProcess()
    {
        CallKomorebic("stop");
    }


    private static void CallKomorebic(string args)
    {
        Debug.WriteLine(args);
        var process = new Process();
        process.StartInfo.WorkingDirectory = Path.Combine(Plugin.GetRunningDictionary(), "vendor", "komorebi");
        process.StartInfo.FileName = "komorebic.exe";
        process.StartInfo.Arguments = args;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        process.Start();
    }

    public static void Subscribe(string pipeName)
    {
        CallKomorebic($"subscribe {pipeName}");
    }

    public static void UnSubscribe(string pipeName = PipeServer.pipeName)
    {
        CallKomorebic($"unsubscribe {pipeName}");
    }

    public static void ChangeWorkSpace(string name = PipeServer.pipeName)
    {
        CallKomorebic($"focus-named-workspace {name}");
    }

    public static void SendFocusedToWorkSpace(string name)
    {
        CallKomorebic($"send-to-named-workspace {name}");
    }
}

public class PipeServer
{
    public const string pipeName = "testpipe233";

    public NamedPipeServerStream? pipeServer;

    public void Create()
    {
        pipeServer =
            new NamedPipeServerStream(pipeName, PipeDirection.InOut);
        Debug.WriteLine("Server started, waiting for connection.");
    }
}