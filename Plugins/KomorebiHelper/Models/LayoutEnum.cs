using System.ComponentModel;

namespace KomorebiHelper.Models;


public enum LayoutEnum
{
    [Description("BSP")] bsp,
    [Description("Columns")] columns,
    [Description("Rows")] rows,
    [Description("VerticalStack")] vertical_stack,
    [Description("HorizontalStack")] horizontal_stack,

    [Description("UltrawideVerticalStack")] ultrawide_vertical_stack
}