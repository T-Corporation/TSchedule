namespace TSchedule.Persistence.Models;

public class ExcelVersion
{
    public int Version { get; set; }

    /// <summary>
    /// Excel 3.0
    /// </summary>
    public static readonly ExcelVersion Excel3 = new() { Version = 3 };

    /// <summary>
    /// Excel 4.0
    /// </summary>
    public static readonly ExcelVersion Excel4 = new() { Version = 4 };

    /// <summary>
    /// Excel 5.0
    /// </summary>
    public static readonly ExcelVersion Excel5 = new() { Version = 5 };

    /// <summary>
    /// Excel 1997-2003
    /// </summary>
    public static readonly ExcelVersion Excel97 = new() { Version = 1997 };

    /// <summary>
    /// Excel 2007-2010
    /// </summary>
    public static readonly ExcelVersion Excel2007 = new() { Version = 2007 };

    /// <summary>
    /// Excel 2013
    /// </summary>
    public static readonly ExcelVersion Excel2013 = new() { Version = 2013 };

    /// <summary>
    /// Excel 2016
    /// </summary>
    public static readonly ExcelVersion Excel2016 = new() { Version = 2016 };

    public static ExcelVersion[] GetAll() =>
    [
        Excel3, Excel4, Excel5, Excel97, Excel2007, Excel2013, Excel2016
    ];

    public override string ToString()
        => Version.ToString();
}
