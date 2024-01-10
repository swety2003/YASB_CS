
using Microsoft.Extensions.Logging;
using PluginSDK;
using PluginSDK.Core;
using System.Dynamic;
using TestPlugin.View;

namespace TestPlugin
{
    public class PluginInfo : IPlugin
    {

        public Version version { get; } = new Version();
        public string url { get; } = "";
        public string author { get; } = "";
        public PluginInfo()
        {
        }

        public static List<TopBarItemInfo> infos { get; } = new List<TopBarItemInfo>()
        {
            //DevTest.info
            DigitalClock.info,
            UserInfo.info,
            WorkSpaceManager.info,

        };



        public string name => "233Test";

        public ILoggerFactory LoggerFactory { get; set; }

        public IEnumerable<object> GetAllTypeInfo()
        {
            return infos;
        }
    }

}
