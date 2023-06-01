using Microsoft.EntityFrameworkCore;

namespace AutoService.Data;

[Index(nameof(PhoneNumber), IsUnique = true)]
public class Mechanic
{
    public  int Id { get; set; }
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}