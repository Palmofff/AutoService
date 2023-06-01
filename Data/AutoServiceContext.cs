using Microsoft.EntityFrameworkCore;
using AutoService.Models;

namespace AutoService.Data;

public class AutoServiceContext : DbContext
{
    public AutoServiceContext(DbContextOptions<AutoServiceContext> options)
        : base(options)
    {
    }
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Mechanic> Mechanics => Set<Mechanic>();
    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
}