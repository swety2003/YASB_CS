using System;
using Avalonia.Controls;
using Newtonsoft.Json;
using TestPlugin.ViewModels;

namespace TestPlugin.Extra;

public static class UserControlEx
{
    public static void InitVM<T>(this UserControl self) where T : ViewModelBase
    {
        object[] parameters = new object[1];
        parameters[0] = self;
        self.DataContext = Activator.CreateInstance(typeof(T),parameters);
    }
    
    public static T GetVM<T>(this UserControl self) where T : ViewModelBase
    {
        if (self.DataContext is T vm)
        {
            return vm;
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}