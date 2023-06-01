using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace AutoService.Models;

public class AnnualReportModel
{
    public int Year { get; set; }
    public int Month { get; set; }
    [DisplayName("Month")]
    public string MonthName
    {
        get
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(this.Month);
        }
    }
    public int? MechanicId { get; set; }
    [DisplayName("Mechanic")]
    public string? MechanicName { get; set; }
    [DisplayName("Total price")]
    public int TotalPrice { get; set; }
    [DisplayName("Number of orders")]
    public int? NumberOfOrders { get; set; }
    [DisplayName("Number of reworks")]
    public int NumberOfReworks { get; set; }
}