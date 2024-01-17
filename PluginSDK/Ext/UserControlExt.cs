using PluginSDK.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PluginSDK.SDK.Extensions
{
    public static class UserControlExt
    {

        public static string GetPluginConfigFilePath(this IViewBase self)
        {
            string ret;

            var abl = Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location);
            if (self is ITopBarItem)
            {
                ret = Path.Combine(abl, "Configs", "config.json");
            }
            else
            {
                throw new NotSupportedException();
            }

            if (!Directory.Exists(Path.GetDirectoryName(ret)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ret)??throw new Exception());
            }
            return ret;
        }

    }
}
