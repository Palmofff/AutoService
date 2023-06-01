using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace AutoService.Data;
public class Car
{
    public string Model { get; set; } = null!;
    public int? YearOfManufacture { get; set; }
    [Key]
    public string? PlateNumber { get; set; }
}