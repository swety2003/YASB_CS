using PluginSDK.Core;
using System.Windows.Controls;
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
