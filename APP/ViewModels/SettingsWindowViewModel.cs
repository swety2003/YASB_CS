using System.Collections.Generic;
using System.Collections.ObjectModel;
using APP.Common;
using APP.Models;
using APP.Shared.Core;

namespace APP.ViewModels;

public class SettingsWindowViewModel : ViewModelBase
{
    public ObservableCollection<WidgetMainfest> CardInfos => Program.GetService<PluginLoader>().WidgetMainfests;

    public IList<WidgetStatus> TopBarStatuses => Program.GetService<WidgetContainerService>().WidgetStatusList;
}