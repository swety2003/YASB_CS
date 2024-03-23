using Avalonia;
using System;
using System.IO;
using System.Reflection;

namespace KomorebiHelper;

class Plugin
{
    public static string GetRunningDictionary()
    {
        return Path.GetDirectoryName(Assembly.GetCallingAssembly().Location) ?? throw new NotSupportedException();
    }
}