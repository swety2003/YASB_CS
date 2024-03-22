using System;

namespace APP.Services;

public static class ServiceManager
{
    public static IServiceProvider Value => Program.AppHost.Services;

    public static T GetService<T>() where T : class
    {
        if (Value.GetService(typeof(T)) is not T service)
            throw new ArgumentException($"{typeof(T)} Service Not Found");
        return service;
    }
}