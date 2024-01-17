using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using TestPlugin.KomorebiController;
using TestPlugin.Model;

namespace TestPlugin.ViewModel
{
    internal partial class WorkSpaceVM : ViewModelBase
    {

        public WorkSpaceVM(UserControl control) : base(control)
        {
            OnActiveChanged += WorkSpaceVM_OnActiveChanged;

        }

        private void WorkSpaceVM_OnActiveChanged(object? sender, bool e)
        {
            if (e)
            {
                Task.Run(Start);
            }
            else
            {
                Stop();
            }
        }

        PipeServer server;

        [ObservableProperty]
        private WorkspaceData focusedWorkspace = null;

        [ObservableProperty]
        private ObservableCollection<WorkspaceData> workspaces = new ObservableCollection<WorkspaceData>();


        [ObservableProperty]
        private windows_item activeWinInfo;

        [RelayCommand]
        private void ChangeWorkSpace(string name)
        {
            CommandSender.ChangeWorkSpace(name);
        }
        [RelayCommand]
        void SendToWorkSpace(string name)
        {
            CommandSender.SendFocusedToWorkSpace(name);
        }
        public void Stop()
        {
            server.pipeServer?.Close();
            CommandSender.UnSubscribe();
            CommandSender.StopProcess();

        }
        public async Task Start()
        {
            if (!CommandSender.Running())
            {
                CommandSender.StartProcess();

                Console.WriteLine("Wait for 3s...");

                await Task.Delay(3000);
            }
            if (server == null)
            {

                server = new PipeServer();
                server.Create();

                CommandSender.Subscribe(PipeServer.pipeName);

                if (!server.pipeServer.IsConnected)
                {
                    await server.pipeServer.WaitForConnectionAsync();
                    Console.WriteLine("Connected");
                }

            }

            if (server.pipeServer == null) throw new Exception();

            StreamReader sr = new StreamReader(server.pipeServer);
            while (!sr.EndOfStream)
            {

                var line = sr.ReadLine();
                try
                {
                    JsonDataRoot data = JsonConvert.DeserializeObject<JsonDataRoot>(line);
                    List<WorkspaceData> datas = new List<WorkspaceData>();
                    foreach (var item in data.state.monitors.elements[0].workspaces.elements)
                    {
                        datas.Add(new WorkspaceData(item.name, item.containers.elements));
                    }
                    Workspaces = new ObservableCollection<WorkspaceData>(datas);


                    Console.WriteLine($"Event:{data.@event.type}");

                    if (data.@event.type == "FocusChange")
                    {
                        Console.WriteLine($"Focus changed:{data.@event.content}");

                        JArray ja = JArray.FromObject(data.@event.content);
                        var itemObj = ja[1];
                        ActiveWinInfo = JsonConvert.DeserializeObject<windows_item>(itemObj.ToString());
                    }



                    var focusedWorkspaceIndex = data.state.monitors.elements[0].workspaces.focused;

                    await Task.Delay(50);
                    FocusedWorkspace = Workspaces[focusedWorkspaceIndex];

                    Console.WriteLine($"Current workspace:{focusedWorkspaceIndex}");




                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    Debugger.Break();


                }

            }
            Console.WriteLine($"Closed");

        }
    }
}
