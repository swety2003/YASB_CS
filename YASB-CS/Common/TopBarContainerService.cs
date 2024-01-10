using Microsoft.VisualBasic;
using Panuon.WPF.UI;
using PluginSDK.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using YASB_CS.Model;
using YASB_CS.ViewModel;

namespace YASB_CS.Common
{
    internal class TopBarContainerService
    {
        internal StackPanel? LeftPanel { get; set; }

        internal StackPanel? CenterPanel { get; set; }

        internal StackPanel? RightPanel { get; set; }

        private Dictionary<TopBarPos, StackPanel> PanelMap { get; set; } = new Dictionary<TopBarPos, StackPanel>();

        public void InitTopBarContainerService()
        {
            PanelMap[TopBarPos.Left] = LeftPanel??throw new Exception();
            PanelMap[TopBarPos.Center] = CenterPanel ?? throw new Exception();
            PanelMap[TopBarPos.Right] = RightPanel ?? throw new Exception();

            foreach (var item in App.GetService<PluginLoader>().TopBarItemInfos)
            {
                topBarStatusList.Add(new TopBarStatus(item.MainView.FullName ?? throw new Exception()));
            }
        }

        private IList<TopBarStatus> topBarStatusList { get; set; } = new List<TopBarStatus>();

        internal IList<TopBarStatus> TopBarStatusList { get { return topBarStatusList; } }

        private IList<ITopBarItem> activeItems { get; set; }= new List<ITopBarItem>();


        internal event PropertyChangedEventHandler? PropertyChanged;


        internal void Enable(TopBarStatus ts,TopBarPos pos)
        {
            var card = Activator.CreateInstance(ts.CardInfo.MainView) as ITopBarItem;

            PanelMap[pos].Children.Add(card as UserControl);
            activeItems.Add(card);

            card.OnEnabled();

            PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(TopBarStatusList)));


        }


        public void Disable(TopBarStatus tbs)
        {
            try
            {
                var c = activeItems.Where(x => x.Info==tbs.CardInfo).First();
                Disable(c,tbs.Pos);
            }
            catch (Exception ex)
            {
                //logger.LogError(ex, "禁用失败");
            }
        }

        public void Disable(ITopBarItem c, TopBarPos pos)
        {

            c.OnDisabled();

            PanelMap[pos].Children.Remove(c as UserControl);

            activeItems.Remove(c);

            PropertyChanged?.Invoke(null, new PropertyChangedEventArgs(nameof(TopBarStatusList)));

        }

    }
}
