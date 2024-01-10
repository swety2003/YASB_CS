using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginSDK.Core
{

    public interface ITopBarItem : IViewBase
    {
        public TopBarItemInfo Info { get; }
    }
    public record TopBarItemInfo(string Name, string Description, Type MainView);
}
