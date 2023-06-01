namespace AutoService.Data;

public class WorkOrder
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;
    public string CarPlateNumber { get; set; } = null!;
    public Car Car { get; set; } = null!;
    public int? MechanicId { get; set; }
    public Mechanic? Mechanic { get; set; }
    public string Description { get; set; } = null!;
    public int? Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? Discount { get; set; }
    public bool ReRepair { get; set; }
}