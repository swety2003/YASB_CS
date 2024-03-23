using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;
using APP.Shared;
using KomorebiHelper.Models;
using KomorebiHelper.ViewModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KomorebiHelper.Utils;

public class KEventHelper
{
    private KomorebiHelperViewModel ViewModel { get; }
    private ILogger<KEventHelper> _logger = Logger.CreateLogger<KEventHelper>();
    private NamedPipeServerStream _serverStream;
    
    public Dictionary<string,Action<JsonDataRoot>> ActionMap= new (); 
    public KEventHelper(NamedPipeServerStream s,KomorebiHelperViewModel vm)
    {
        ViewModel = vm;
        _serverStream = s;
        
        init();
    }

    private void init()
    {
        ActionMap.Add("FocusChange", data =>
        {
            _logger.LogDebug($"Focus changed:{data.@event.content}");

            var ja = JArray.FromObject(data.@event.content);
            var itemObj = ja[1];
            ViewModel.ActiveWinInfo = JsonConvert.DeserializeObject<windows_item>(itemObj.ToString());
        });
        ActionMap.Add("FocusNamedWorkspace", data =>
        {
                ViewModel.FocusedWorkspaceIndex = data.state.monitors.elements[0].workspaces.focused;
                _logger.LogDebug($"focused:{ViewModel.FocusedWorkspaceIndex}");
        });
    }

    public async void Watch()
    {
        if (!_serverStream.IsConnected)
        {
            await _serverStream.WaitForConnectionAsync();
            _logger.LogDebug("Connected");
        }
        
        if (_serverStream == null) throw new Exception();

        var sr = new StreamReader(_serverStream);
        while (!sr.EndOfStream)
        {
            await Task.Yield();
            var line = await sr.ReadLineAsync();
            try
            {
                var data = JsonConvert.DeserializeObject<JsonDataRoot>(line);

                if (ViewModel.WorkspaceData == null)
                {
                    
                    var datas = new List<WorkspaceItem>();
                    foreach (var item in data.state.monitors.elements[0].workspaces.elements)
                        datas.Add(new WorkspaceItem(item.name, item.containers.elements, item.layout.Default));
                    ViewModel.WorkspaceData = new WorkspaceData(ViewModel.FocusedWorkspaceIndex, datas);
                    
                    ViewModel.FocusedWorkspaceIndex = data.state.monitors.elements[0].workspaces.focused;

                }

                var ls = ViewModel.WorkspaceData.workspaceItems[ViewModel.FocusedWorkspaceIndex].layout;
                foreach (var item in ViewModel.LayoutList)
                    if (ls == item.Description)
                        ViewModel.Layout = (LayoutEnum)item.Value;


                _logger.LogDebug($"Event:{data.@event.type}");

                if (ActionMap.ContainsKey(data.@event.type))
                {
                    ActionMap[data.@event.type].Invoke(data);
                }
                else
                {
                    _logger.LogWarning($"Unhandled kevent: {data.@event.type}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                Debugger.Break();
            }
        }
    }
}