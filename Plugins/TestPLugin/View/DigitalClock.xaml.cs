using PluginSDK.Core;
using System.Windows.Controls;
using TestPlugin.ViewModel;

namespace TestPlugin.View
{
    /// <summary>
    /// DigitalClock.xaml 的交互逻辑
    /// </summary>
    public partial class DigitalClock : UserControl, ITopBarItem
    {
        internal static TopBarItemInfo info = new("数字时钟", "", typeof(DigitalClock));
        readonly DigitalClockVM vm;
        public DigitalClock()
        {
            InitializeComponent();
            vm = new DigitalClockVM(this);
            this.DataContext = vm;
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
    }
}
