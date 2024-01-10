using CommunityToolkit.Mvvm.ComponentModel;
using PluginSDK.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASB_CS.Common;
using YASB_CS.Model;

namespace YASB_CS.ViewModel
{
    internal partial class ItemManageVM:ObservableObject
    {

        public ObservableCollection<TopBarItemInfo> CardInfos => App.GetService<PluginLoader>().TopBarItemInfos;

        public IList<TopBarStatus> TopBarStatuses => App.GetService<TopBarContainerService>().TopBarStatusList;

    }
}
