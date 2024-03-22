using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using APP.Models;
using APP.Services;
using APP.Shared;
using static APP.Services.ServiceManager;

namespace APP.ViewModels;

public class SettingsWindowViewModel : ViewModelBase
{
    public ObservableCollection<WidgetMainfest> CardInfos => GetService<PluginLoader>().WidgetMainfests;

    public IList<WidgetProfile> TopBarStatuses => GetService<WidgetManager>().WidgetStatusList;

}