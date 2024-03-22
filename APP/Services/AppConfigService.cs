using System.IO;
using APP.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace APP.Services;

internal class AppConfigManager
{
    private const string CONFIG_FILE = "config.json";
    private readonly ILogger<AppConfigManager> _logger;

    public AppConfigManager(ILogger<AppConfigManager> logger)
    {
        _logger = logger;
    }

    internal AppConfig Config { get; set; } = new();

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
        File.WriteAllText(CONFIG_FILE, JsonConvert.SerializeObject(Config));
    }
}