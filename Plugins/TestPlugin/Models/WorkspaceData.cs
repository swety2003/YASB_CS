using System.Collections.Generic;

namespace TestPlugin.Model;

public record WorkspaceItem(string name, List<container_item> wins, string layout);

public record WorkspaceData(int focused, List<WorkspaceItem> workspaceItems);