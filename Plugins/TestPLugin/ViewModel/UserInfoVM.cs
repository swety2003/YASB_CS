using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Controls;

namespace TestPlugin.ViewModel
{
    internal partial class UserInfoVM : ViewModelBase
    {
        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string machineName;

        public UserInfoVM(UserControl control) : base(control)
        {
            UserName = Environment.UserName;
            machineName = Environment.MachineName;
        }
    }
}
