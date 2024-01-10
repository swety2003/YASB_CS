using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YASB_CS.Model;

namespace YASB_CS.Common
{

    public class AppConfigManager
    {

        private readonly ILogger<AppConfigManager> _logger;
        public AppConfigManager(ILogger<AppConfigManager> logger)
        {
            _logger = logger;
        }
        const string CONFIG_FILE = "config.json";

        internal AppConfig Config { get;set; } = new AppConfig();

        internal void Load()
        {

            if (File.Exists(CONFIG_FILE))
            {
                _logger.LogDebug("找到了配置文件");
                Config = JsonConvert.DeserializeObject<AppConfig>(File.ReadAllText(CONFIG_FILE)) ?? new AppConfig();
            }
            else
            {
                _logger.LogWarning("无配置文件");

                Config = new AppConfig();
            }
        }

        internal void Save()
        {
            //var createdCards = App.GetService<CardManageVM>().CreatedCards;

            //Config.CardInstances = createdCards;

            File.WriteAllText(CONFIG_FILE, JsonConvert.SerializeObject(Config));
        }
    }
}
