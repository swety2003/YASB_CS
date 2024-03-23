using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using APP.Shared.Extra;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using KomorebiHelper.Models;
using KomorebiHelper.Utils;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TB.Shared.Utils;
using EnumDescription = KomorebiHelper.Utils.EnumDescription;

namespace KomorebiHelper.ViewModels;

public partial class KomorebiHelperViewModel:ViewModelBase
{
    private ILogger<KomorebiHelperViewModel> _logger = APP.Shared.Logger.CreateLogger<KomorebiHelperViewModel>();
    public override void Init()
    {
        base.Init();

        Task.Run(Start);
    }

    public override void Update()
    {
        
    }
    
    [ObservableProperty] private windows_item activeWinInfo;

    [ObservableProperty] private int focusedWorkspaceIndex;

    [ObservableProperty] private LayoutEnum layout = LayoutEnum.bsp;

    // [ObservableProperty] private WorkspaceItem focusedItem;

    private PipeServer server;


    [ObservableProperty] private WorkspaceData workspaceData;

    public IEnumerable<EnumDescription> LayoutList =>
        Enum.GetValues(typeof(LayoutEnum))
            .Cast<LayoutEnum>()
            .Select(e => new EnumDescription(e));

    
    public void ChangeWorkSpace(string name)
    {
        CommandHelper.ChangeWorkSpace(name);
    }

    public void SendToWorkSpace(string name)
    {
        CommandHelper.SendFocusedToWorkSpace(name);
    }

    public void Stop()
    {
        server.PipeServerStream?.Close();
        CommandHelper.UnSubscribe();
        CommandHelper.RestoreWindows();
        CommandHelper.StopProcess();
    }

    public async Task Start()
    {
        if (CommandHelper.Running())
        {
            CommandHelper.UnSubscribe();
            CommandHelper.StopProcess();
        }

        CommandHelper.StartProcess();

        _logger.LogDebug("Wait for 1s...");

        await Task.Delay(1000);

        if (server == null)
        {
            server = new PipeServer();
            server.Create();

            CommandHelper.Subscribe(PipeServer.pipeName);

        }

        CommandHelper.SetWinHideBehavior();
        // CommandHelper.ToggleMonocle();

        new KEventHelper(server.PipeServerStream,this).Watch();

    }
}