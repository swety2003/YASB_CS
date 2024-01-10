using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using TestPlugin.KomorebiController;
using TestPlugin.Model;
using TestPlugin.View;

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

        //[ObservableProperty]
        //private int focusedWorkspaceIndex=-1;
        private int focusedWorkspaceIndex;

        public int FocusedWorkspaceIndex
        {
            get { return focusedWorkspaceIndex; }
            set 
            {
                SetProperty(ref focusedWorkspaceIndex, value);
            }
        }




        [ObservableProperty]
        private ObservableCollection<WorkspaceData> workspaces = new ObservableCollection<WorkspaceData>();

        [ObservableProperty]
        private windows_item activeWinInfo;

        [RelayCommand]
        private void ChangeWorkSpace(string name)
        {
            CommandSender.ChangeWorkSpace(name);
        }
        public void Stop()
        {
            //server.pipeServer.Close();
            CommandSender.UnSubscribe();

        }
        public async Task Start()
        {
            if(server== null)
            {

                server = new PipeServer();
                server.Create();

                CommandSender.Subscribe(PipeServer.pipeName);

                if(!server.pipeServer.IsConnected)
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
                        datas.Add(new WorkspaceData(item.name));
                    }
                    Workspaces = new ObservableCollection<WorkspaceData>(datas);


                    if (data.@event.type == "FocusChange")
                    {
                        Console.WriteLine($"Focus changed:{data.@event.content}");

                        JArray ja = JArray.FromObject(data.@event.content);
                        var itemObj = ja[1];
                        ActiveWinInfo = JsonConvert.DeserializeObject<windows_item>(itemObj.ToString());
                    }


                    Console.WriteLine($"Current workspace:{data.state.monitors.elements[0].workspaces.focused}");

                    FocusedWorkspaceIndex = data.state.monitors.elements[0].workspaces.focused;






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
