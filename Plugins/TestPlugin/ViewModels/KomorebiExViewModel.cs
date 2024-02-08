using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using APP.Shared.Extra;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestPlugin.Extra;
using TestPlugin.Model;
using TestPlugin.Models.Enums;

namespace TestPlugin.ViewModels;

public partial class KomorebiExViewModel : ViewModelBase
{
    [ObservableProperty] private windows_item activeWinInfo;

    [ObservableProperty] private int focusedWorkspaceIndex;

    [ObservableProperty] private LayoutEnum layout = LayoutEnum.bsp;


    private PipeServer server;


    [ObservableProperty] private WorkspaceData workspaceData;

    public KomorebiExViewModel(UserControl control) : base(control)
    {
    }

    public IEnumerable<EnumDescription> LayoutList =>
        Enum.GetValues(typeof(LayoutEnum))
            .Cast<LayoutEnum>()
            .Select(e => new EnumDescription(e));

    internal override void OnEnabled()
    {
        Task.Run(Start);
    }

    internal override void OnDisabled()
    {
        Stop();
    }

    
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
        server.pipeServer?.Close();
        CommandHelper.UnSubscribe();
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

        Debug.WriteLine("Wait for 3s...");

        await Task.Delay(3000);

        if (server == null)
        {
            server = new PipeServer();
            server.Create();

            CommandHelper.Subscribe(PipeServer.pipeName);

            if (!server.pipeServer.IsConnected)
            {
                await server.pipeServer.WaitForConnectionAsync();
                Debug.WriteLine("Connected");
            }
        }

        CommandHelper.SetWinHideBehavior();
        // CommandHelper.ToggleMonocle();

        if (server.pipeServer == null) throw new Exception();

        var sr = new StreamReader(server.pipeServer);
        while (!sr.EndOfStream)
        {
            await Task.Yield();
            var line = sr.ReadLine();
            try
            {
                var data = JsonConvert.DeserializeObject<JsonDataRoot>(line);
                FocusedWorkspaceIndex = data.state.monitors.elements[0].workspaces.focused;
                var datas = new List<WorkspaceItem>();

                foreach (var item in data.state.monitors.elements[0].workspaces.elements)
                    datas.Add(new WorkspaceItem(item.name, item.containers.elements, item.layout.Default));
                Debug.WriteLine($"focused:{FocusedWorkspaceIndex}");
                WorkspaceData = new WorkspaceData(FocusedWorkspaceIndex, datas);

                var ls = WorkspaceData.workspaceItems[FocusedWorkspaceIndex].layout;
                foreach (var item in LayoutList)
                    if (ls == item.Description)
                        Layout = (LayoutEnum)item.Value;


                Debug.WriteLine($"Event:{data.@event.type}");

                if (data.@event.type == "FocusChange")
                {
                    Debug.WriteLine($"Focus changed:{data.@event.content}");

                    var ja = JArray.FromObject(data.@event.content);
                    var itemObj = ja[1];
                    ActiveWinInfo = JsonConvert.DeserializeObject<windows_item>(itemObj.ToString());
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                Debugger.Break();
            }
        }
    }
}