using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using Microsoft.Extensions.Logging;

namespace KomorebiHelper.Utils;


public static class CommandHelper
{
    private static ILogger _logger = APP.Shared.Logger.LoggerFactory.CreateLogger("CommandHelper");
    
    public static void StartProcess()
    {
        var config_file = Path.Combine(Plugin.GetRunningDictionary(), "config", "komorebi", "komorebi.json");
        CallKomorebic($"start");
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
    public static void ToggleMonocle()
    {
        CallKomorebic("toggle-monocle");
    }
    public static void StopProcess()
    {
        CallKomorebic("stop");
    }

    private const string K_ROOT = @"C:\Applications\komorebi-0.1.22-x86_64-pc-windows-msvc";
    private static void CallKomorebic(string args)
    {
        _logger.LogDebug(args);
        var process = new Process();
        process.StartInfo.WorkingDirectory = Path.Combine(K_ROOT);
        process.StartInfo.FileName =Path.Combine(K_ROOT, "komorebic.exe");
        process.StartInfo.Arguments = args;
        process.StartInfo.UseShellExecute = false;
        process.StartInfo.CreateNoWindow = true;
        try
        {
            process.Start();
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }
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

    public static void RestoreWindows()
    {
        CallKomorebic($"restore-windows");
    }
}

public class PipeServer
{
    private static ILogger _logger = APP.Shared.Logger.LoggerFactory.CreateLogger("PipeServer");

    public const string pipeName = "testpipe233";

    public NamedPipeServerStream? PipeServerStream;

    public void Create()
    {
        PipeServerStream =
            new NamedPipeServerStream(pipeName, PipeDirection.InOut);
            _logger.LogDebug($"PipeServer {pipeName} started, waiting for connection.");
    }
}