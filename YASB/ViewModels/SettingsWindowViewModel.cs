using System.Collections.Generic;
using System.Collections.ObjectModel;
using APP.SDK.Core;
using YASB.Common;
using YASB.Models;

namespace YASB.ViewModels;

public class SettingsWindowViewModel : ViewModelBase
{
    public ObservableCollection<WidgetMainfest> CardInfos => Program.GetService<PluginLoader>().WidgetMainfests;

    public IList<WidgetStatus> TopBarStatuses => Program.GetService<WidgetContainerService>().WidgetStatusList;
}