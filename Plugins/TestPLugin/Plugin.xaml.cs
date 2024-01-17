
using Microsoft.Extensions.Logging;
using PluginSDK;
using PluginSDK.Core;
using System.IO;
using TestPlugin.View;

namespace TestPlugin
{
    public class Plugin : IPlugin
    {
        public static string GetRunningDictionary()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetCallingAssembly().Location) ?? throw new NotSupportedException();
        }

        public Version version { get; } = new Version();
        public string url { get; } = "";
        public string author { get; } = "";
        public Plugin()
        {
        }

        public static List<TopBarItemInfo> infos { get; } = new List<TopBarItemInfo>()
        {
            //DevTest.info
            DigitalClock.info,
            WorkSpaceManager.info,
            HardwareMonitor.info,
            UserInfo.info,

        };



        public string name => "233Test";

        public ILoggerFactory LoggerFactory { get; set; }

        public IEnumerable<object> GetAllTypeInfo()
        {
            return infos;
        }
    }

}
