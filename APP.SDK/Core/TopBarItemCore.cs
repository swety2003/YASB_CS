using System;

namespace APP.SDK.Core;

public interface IWidgetItem : IViewBase
{
    public WidgetMainfest Info { get; }
}

public record WidgetMainfest(string Name, string Description, Type Widget);