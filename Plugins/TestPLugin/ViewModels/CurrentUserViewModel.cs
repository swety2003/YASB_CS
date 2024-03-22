using System;
using System.Diagnostics;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using TB.Shared.Utils;

namespace TestPlugin.ViewModels;

public partial class CurrentUserViewModel : ViewModelBase
{
    [ObservableProperty] private string machineName;

    [ObservableProperty] private string userName;


    public override void Init()
    {
        base.Init();
        
        UserName = Environment.UserName;
        machineName = Environment.MachineName;
    }


    public void ShutDown()
    {
        RUN_COMMAND("shutdown /s /t 0");
    }

    public void ReBoot()
    {
        RUN_COMMAND("shutdown -r -t 0");
    }

    public void LogOut()
    {
        RUN_COMMAND("logout");
    }

    public void Lock()
    {
        RUN_COMMAND("rundll32.exe user32.dll LockWorkStation");
    }

    public void AdvancedBoot()
    {
        RUN_COMMAND("shutdown /r /o /f /t 00");
    }

    private void RUN_COMMAND(string cmd)
    {
        // 创建一个新的进程启动信息实例  
        var psi = new ProcessStartInfo();
        // 设置要启动的程序（这里是cmd.exe）  
        psi.FileName = "cmd.exe";
        // 设置命令行参数（这里是关机命令）  
        psi.Arguments = $"/c {cmd}";
        // 设置窗口风格（这里是不显示窗口）  
        psi.WindowStyle = ProcessWindowStyle.Hidden;
        // 设置使用Shell执行  
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;

        // 创建一个新的进程实例并启动它  
        using (var process = new Process())
        {
            process.StartInfo = psi;
            process.Start();
            // 等待进程结束  
            process.WaitForExit();
        }
    }


    public override void Update()
    {
    }
}