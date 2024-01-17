using PluginSDK.Core;
using System.Dynamic;
using System.Windows.Controls;
using TestPlugin.ViewModel;

namespace TestPlugin.View
{
    /// <summary>
    /// CPUMonitor.xaml 的交互逻辑
    /// </summary>
    public partial class HardwareMonitor : UserControl,ITopBarItem
    {
        public static TopBarItemInfo info => new TopBarItemInfo( "资源监控", "硬件占用监控", typeof(HardwareMonitor));

        public TopBarItemInfo Info => info;

        private readonly HWMonitorVM vm;

        public HardwareMonitor()
        {
            InitializeComponent();

            vm = new HWMonitorVM(this);
            DataContext = vm;
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
