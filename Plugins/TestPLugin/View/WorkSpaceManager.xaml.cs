using PluginSDK.Core;
using System.Windows;
using System.Windows.Controls;
using TestPlugin.ViewModel;

namespace TestPlugin.View
{
    /// <summary>
    /// WorkSpaceManager.xaml 的交互逻辑
    /// </summary>
    public partial class WorkSpaceManager : UserControl, ITopBarItem
    {
        internal static TopBarItemInfo info = new("工作空间管理", "", typeof(WorkSpaceManager));

        WorkSpaceVM vm;
        public WorkSpaceManager()
        {
            InitializeComponent();
            vm = new WorkSpaceVM(this);
            DataContext = vm;
        }

        public TopBarItemInfo Info => info;

        public void OnDisabled()
        {
            vm.Active = false;
        }

        public void OnEnabled()
        {

            vm.Active = true;
        }


        private void ListBox_Selected(object sender, RoutedEventArgs e)
        {

        }
    }
}
