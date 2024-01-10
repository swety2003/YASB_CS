using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YASB_CS.Common;
using YASB_CS.Model;
using YASB_CS.ViewModel;

namespace YASB_CS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            DataContext = App.GetService<MainWindowVM>();
            base.OnSourceInitialized(e);
            TopBarContainerService tbcs= App.GetService<TopBarContainerService>();
            tbcs.RightPanel = right_area;
            tbcs.LeftPanel = left_area;
            tbcs.CenterPanel = center_area;
            tbcs.InitTopBarContainerService();


            var tbs = App.GetService<ItemManageVM>().TopBarStatuses;
            foreach (var item in App.GetService<AppConfigManager>().Config.Status ?? new List<TopBarStatus>())
            {
                foreach (var i1 in tbs)
                {
                    if (item.Enabled && i1.Wid == item.Wid)
                    {
                        i1.Pos=item.Pos;
                        i1.Enabled = true;
                    }
                }
            }

        }

        
    }
}