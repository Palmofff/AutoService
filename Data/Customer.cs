using System.Security.Policy;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Data;
[Index(nameof(PhoneNumber), IsUnique = true)]
public class Customer
{
    public  int Id { get; set; }
    public string Name { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}