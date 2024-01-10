
using Microsoft.Extensions.Logging;

namespace PluginSDK
{

    public interface IPlugin
    {
        public string name { get; }
        public Version version { get; }
        public string url { get; }
        public string author { get; }

        public IEnumerable<object> GetAllTypeInfo();

    }

    public interface IViewBase
    {
        public void OnEnabled();
        public void OnDisabled();

    }



    /// <summary>
    /// 
    /// </summary>
    public static class Logger
    {

        private static ILoggerFactory? _loggerFactory;

        public static ILoggerFactory? LoggerFactory
        {
            get { return _loggerFactory; }
            set
            {
                if (_loggerFactory == null)
                {
                    _loggerFactory = value;
                }
            }
        }

        public static ILogger<T> CreateLogger<T>()
        {
            return LoggerFactory?.CreateLogger<T>() ?? throw new Exception("Logger.LoggerFactory Œ¥≥ı ºªØ£°");
        }

    }

}
