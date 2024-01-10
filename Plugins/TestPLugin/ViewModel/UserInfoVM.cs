using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TestPlugin.ViewModel
{
    internal partial class UserInfoVM:ViewModelBase
    {
        [ObservableProperty]
        private string userName;

        [ObservableProperty]
        private string machineName;

        public UserInfoVM(UserControl control) :base(control)
        {
            UserName = Environment.UserName;
            machineName = Environment.MachineName;
        }
    }
}
