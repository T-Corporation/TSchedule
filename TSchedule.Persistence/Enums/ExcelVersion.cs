using System.ComponentModel;
using TSchedule.Persistence.Extensions;

namespace TSchedule.Persistence.Enums;

public enum ExcelVersion
{
    [Description("Excel 97-2003"), Extension(".xls")]
    Excel97,
    [Description("Excel 2007-2016"), Extension(".xlsx")]
    Excel2007
}
