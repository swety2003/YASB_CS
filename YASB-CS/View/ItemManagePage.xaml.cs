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
using YASB_CS.ViewModel;

namespace YASB_CS.View
{
    /// <summary>
    /// ItemManagePage.xaml 的交互逻辑
    /// </summary>
    public partial class ItemManagePage : Page
    {
        public ItemManagePage()
        {
            InitializeComponent();
            DataContext = App.GetService<ItemManageVM>();
        }
    }
}
