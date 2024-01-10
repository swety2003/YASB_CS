using Microsoft.VisualBasic;
using Newtonsoft.Json;
using PluginSDK.Core;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using YASB_CS.Common;

namespace YASB_CS.Model
{
    public class TopBarStatus
    {

        [JsonProperty("WID")]
        public string Wid { get; private set; }

        private TopBarPos posProperty = TopBarPos.Left;
        public TopBarPos Pos
        {
            get { return posProperty; }
            set
            {
                if (Enabled)
                {
                    App.GetService<TopBarContainerService>().Disable(this);

                    App.GetService<TopBarContainerService>().Enable(this, value);
                }

                posProperty = value;
            }
        }

        [JsonProperty("Enabled")]
        private bool enabledProperty = false;
        [JsonIgnore]
        public bool Enabled
        {
            get { return enabledProperty; }
            set
            {
                enabledProperty = value;
                if (value)
                {
                    App.GetService<TopBarContainerService>().Enable(this,Pos);
                }
                else
                {
                    App.GetService<TopBarContainerService>().Disable(this);
                }
            }
        }

        public TopBarStatus(string wid,TopBarPos pos=TopBarPos.Left)
        {
            Wid=wid;
            Pos=pos;
        }

        [JsonIgnore]
        public TopBarItemInfo CardInfo => App.GetService<PluginLoader>().TopBarItemInfos
            .Where(x => x.MainView.FullName == Wid).Select(x => x).First();
    }
}
