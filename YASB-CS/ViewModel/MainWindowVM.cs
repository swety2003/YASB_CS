using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.VisualBasic;
using Panuon.WPF.UI;
using PluginSDK.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using YASB_CS.Common;
using YASB_CS.Controls.Docking;
using YASB_CS.Model;
using YASB_CS.View;

namespace YASB_CS.ViewModel
{
    internal partial class MainWindowVM: ObservableObject
    {
        public MainWindowVM()
        {

        }
        private AppConfig Config => App.GetService<AppConfigManager>().Config;

        private readonly TopBarContainerService topBarContainerService = App.GetService<TopBarContainerService>();

        [RelayCommand]
        private void ShowSetting()
        {
            App.GetService<SettingWindow>().Show();
        }

        [RelayCommand]
        private void ExitApp()
        {
            var w = Application.Current.MainWindow;
            DesktopAppBar.SetAppBar(w, AppBarEdge.None);
            Application.Current.Shutdown();
        }




    }
}
