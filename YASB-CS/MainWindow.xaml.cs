using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using YASB_CS.Common;
using YASB_CS.Controls.Docking;
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

            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
        }


        #region Check fullscreen app

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            RegisterAppBar(true);
            DesktopAppBar.SetAppBar(this, AppBarEdge.None);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterAppBar(false);
        }
        private bool runningFullScreenApp;

        public bool RunningFullScreenApp
        {
            get { return  runningFullScreenApp; }
            set 
            {  
                if (runningFullScreenApp == value) { return; }
                runningFullScreenApp = value; 
                if (runningFullScreenApp)
                {
                    this.WindowState=WindowState.Minimized;
                    //DesktopAppBar.SetAppBar(this, AppBarEdge.None);
                }
                else
                {

                    this.WindowState = WindowState.Normal;

                    DesktopAppBar.SetAppBar(this, AppBarEdge.Top);
                    //DesktopAppBar.SetAppBar(this, AppBarEdge.Top);

                }
            }
        }

        private IntPtr desktopHandle;
        private IntPtr shellHandle;
        int uCallBackMsg;
        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == uCallBackMsg)
            {
                switch (wParam.ToInt32())
                {
                    case (int)ABNotify.ABN_FULLSCREENAPP:
                        {
                            IntPtr hWnd = APIWrapper.GetForegroundWindow();
                            //判断当前全屏的应用是否是桌面
                            if (hWnd.Equals(desktopHandle) || hWnd.Equals(shellHandle))
                            {
                                RunningFullScreenApp = false;
                                break;
                            }
                            //判断是否全屏
                            if ((int)lParam == 1)
                                this.RunningFullScreenApp = true;
                            else
                                this.RunningFullScreenApp = false;
                            break;
                        }
                    default:
                        break;
                }
            }
            return IntPtr.Zero;
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



            HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
            source.AddHook(WndProc);

            WinHide.Hide(this);

        }
        private void RegisterAppBar(bool registered)
        {
            APPBARDATA abd = new APPBARDATA();
            abd.cbSize = Marshal.SizeOf(abd);
            abd.hWnd = new WindowInteropHelper(this).Handle;

            desktopHandle = APIWrapper.GetDesktopWindow();
            shellHandle = APIWrapper.GetShellWindow();
            if (!registered)
            {
                //register
                uCallBackMsg = APIWrapper.RegisterWindowMessage("APPBARMSG_CSDN_HELPER");
                abd.uCallbackMessage = uCallBackMsg;
                uint ret = APIWrapper.SHAppBarMessage((int)ABMsg.ABM_NEW, ref abd);
            }
            else
            {
                APIWrapper.SHAppBarMessage((int)ABMsg.ABM_REMOVE, ref abd);
            }
        }

        #endregion


    }
}