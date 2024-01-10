using PluginSDK.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TestPlugin.ViewModel;

namespace TestPlugin.View
{
    /// <summary>
    /// UserDisplay.xaml 的交互逻辑
    /// </summary>
    public partial class UserInfo : UserControl, ITopBarItem
    {
        internal static TopBarItemInfo info = new("用户信息", "", typeof(UserInfo));
        readonly UserInfoVM vm;

        public TopBarItemInfo Info => info;
        public UserInfo()
        {
            InitializeComponent();
            vm = new UserInfoVM(this);
            this.DataContext = vm;
        }


        public void OnDisabled()
        {
            vm.Active = false;
        }

        public void OnEnabled()
        {
            vm.Active = true;
        }
    }
}
