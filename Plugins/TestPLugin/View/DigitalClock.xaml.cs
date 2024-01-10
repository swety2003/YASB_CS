using PluginSDK;
using PluginSDK.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
    /// DigitalClock.xaml 的交互逻辑
    /// </summary>
    public partial class DigitalClock :UserControl, ITopBarItem
    {
        internal static TopBarItemInfo info = new("数字时钟", "", typeof(DigitalClock));
        readonly DigitalClockVM vm;
        public DigitalClock()
        {
            InitializeComponent();
            vm=new DigitalClockVM(this);
            this.DataContext= vm;
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
