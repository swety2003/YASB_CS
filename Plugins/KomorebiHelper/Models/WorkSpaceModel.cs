using System.Collections.Generic;
using KomorebiHelper.Utils;

namespace KomorebiHelper.Models;


public record WorkspaceItem(string name, List<container_item> wins, string layout)
{
   public void Switch()
    {
        CommandHelper.ChangeWorkSpace(name);
    }
};

public record WorkspaceData(int focused, List<WorkspaceItem> workspaceItems);