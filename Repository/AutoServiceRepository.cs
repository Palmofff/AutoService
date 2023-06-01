using AutoService.Data;
using AutoService.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoService.Repository;

public class AutoServiceRepository : IAutoServiceRepository
{
    private readonly AutoServiceContext _context;

    public AutoServiceRepository(AutoServiceContext context)
    {
        _context = context;
    }

    public async Task AddNewMechanic(Mechanic mechanic)
    {
        _context.Add(mechanic);
        await _context.SaveChangesAsync();
    }
    

    public async Task<List<WorkOrderModel?>> GetAllOrdersAsync()
    {
        var ordersList = await _context.WorkOrders
            .Include(w => w.Mechanic)
            .Include(w => w.Car)
            .Include(w => w.Customer)
            .ToListAsync();
        return ordersList.Select(WorkOrderModel.FromWorkOrder).ToList();
        // var models = new List<WorkOrderModel>();
        // foreach (var order in ordersList)
        // {
        //     models.Add(WorkOrderModel.FromWorkOrder(order));
        // }
        // return models;
    }

    public async Task<List<WorkOrderModel?>> GetOrdersWithoutMechanicAsync()
    {
        var noMechanicList = await _context.WorkOrders
            .Where(w => w.MechanicId == null)
            .Include(w => w.Mechanic)
            .Include(w => w.Car)
            .Include(w => w.Customer)
            .ToListAsync();
        return noMechanicList.Select(WorkOrderModel.FromWorkOrder).ToList();
    }

    public async Task<List<WorkOrderModel?>> GetOrderWithoutPriceAsync()
    {
        var noPrice = await _context.WorkOrders
            .Where(w => w.Price == null)
            .Include(w => w.Mechanic)
            .Include(w => w.Car)
            .Include(w => w.Customer)
            .ToListAsync();
        return noPrice.Select(WorkOrderModel.FromWorkOrder).ToList();
    }

    public async Task<List<WorkOrderModel?>> UnfinishedOrdersAsync()
    {
        var unfinished = await _context.WorkOrders
            .Include(w => w.Mechanic)
            .Include(w => w.Car)
            .Include(w => w.Customer)
            .Where(w => w.EndDate == null && w.Mechanic != null)
            .ToListAsync();
        return unfinished.Select(WorkOrderModel.FromWorkOrder).ToList();
    }

    public async Task<WorkOrderModel?> GetWorkOrderByIdAsync(int id)
    {
        var order = await _context.WorkOrders
            .Include(w => w.Mechanic)
            .Include(w => w.Car)
            .Include(w => w.Customer)
            .FirstOrDefaultAsync(w => w.Id == id);
        return WorkOrderModel.FromWorkOrder(order);
    }

    public async Task<WorkOrder?> GetWorkOrderById(int id)
    {
        return await _context.WorkOrders
            .Include(w => w.Mechanic)
            .Include(w => w.Car)
            .Include(w => w.Customer)
            .FirstOrDefaultAsync(w => w.Id == id);
    }
    public async Task UpdateWorkOrderAsync(WorkOrderModel workOrderModel)
    {
        var order = await GetWorkOrderById(workOrderModel.Id);
        order.MechanicId = workOrderModel.MechanicId;
        order.Mechanic = await GetMechanicAsync(workOrderModel.MechanicId);
        _context.Update(order);
        await _context.SaveChangesAsync();
    }
    public async Task FinishOrderAsync(WorkOrderModel workOrderModel)
    {
        var order = await GetWorkOrderById(workOrderModel.Id);
        order.Price = workOrderModel.Price;
        order.EndDate = workOrderModel.EndDate;
        _context.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task AddNewWorkOrder(WorkOrderModel workOrderModel)
    {
        var customer = await CheckForCustomerAsync(workOrderModel);
        var car = await CheckForCarAsync(workOrderModel);
        var workOrderToCreate = workOrderModel.ToWorkOrder();
        workOrderToCreate.Car = car;
        workOrderToCreate.Customer = customer;
        _context.WorkOrders.Add(workOrderToCreate);
        await _context.SaveChangesAsync();
    }

    public async Task<Customer> CheckForCustomerAsync(WorkOrderModel workOrderModel)
    {
        var customerToCheck = await _context.Customers
            .FirstOrDefaultAsync(c => c.PhoneNumber == workOrderModel.PhoneNumber);
        if (customerToCheck == null)
        {
            var customer = new Customer()
            {
                Id = workOrderModel.CustomerId,
                PhoneNumber = workOrderModel.PhoneNumber,
                Name = workOrderModel.CustomerName
            };
             _context.Customers.Add(customer);
             await _context.SaveChangesAsync();
             return customer;
        }
        return customerToCheck;
    }

    public async Task<Car> CheckForCarAsync(WorkOrderModel workOrderModel)
    {
        var carToCheck =await _context.Cars
            .FirstOrDefaultAsync(c => c.PlateNumber == workOrderModel.CarPlateNumber);
        if (carToCheck == null)
        {
            var car = new Car()
            {
                PlateNumber = workOrderModel.CarPlateNumber,
                Model = workOrderModel.CarModel,
                YearOfManufacture = workOrderModel.CarYear
            };
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return car;
        }

        return carToCheck;
    }
    
    public async Task<WorkOrderModel> InitializeWorkOrderModel()
    {
        return new WorkOrderModel
        {
            AvailableMechanics = await GetAvailableMechanicsFromDb()
        };
    }
    
    public async Task GetAvailableMechanics(WorkOrderModel workOrderModel)
    {
        workOrderModel.AvailableMechanics = await GetAvailableMechanicsFromDb();
    }

    public async Task<Car?> GetCarAsync(string plateNumber)
    {
        return await _context.Cars.Where(c => c.PlateNumber == plateNumber)
            .FirstOrDefaultAsync();
    }

    public async Task<Customer?> GetCustomerAsync(int id)
    {
        return await _context.Customers.Where(c => c.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Mechanic?> GetMechanicAsync(int? id)
    {
        return await _context.Mechanics.Where(c => c.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task EditMechanicAsync(Mechanic mechanic)
    {
        _context.Mechanics.Update(mechanic);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Mechanic>> GetAllMechanicsAsync()
    {
        return await _context.Mechanics.ToListAsync();
    }

    public async Task<List<AnnualReportModel>> GetReportAsync()
    {
        
        return await _context.WorkOrders
            .Where(w => w.EndDate != null && w.MechanicId != null )
            .GroupBy(x => new { Year = x.EndDate.Value.Year, Month = x.EndDate!.Value.Month, MechanicId = x.MechanicId  })
            .Select(g => new AnnualReportModel
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                MechanicId = g.Key.MechanicId,
                MechanicName = g.FirstOrDefault()!.Mechanic!.Name,
                TotalPrice = g.Sum(x => x.Price ?? 0),
                NumberOfOrders = g.Count(),
                NumberOfReworks = g.Count(x => x.ReRepair)
            }).OrderByDescending(a => a.Year)
            .ThenByDescending(a=> a.Month).Reverse()
            .ToListAsync();
    }

    private async Task<List<SelectListItem>> GetAvailableMechanicsFromDb()
    {
        var mechanicsList = await _context.Mechanics.ToListAsync();
        var returnList = new List<SelectListItem> { new() };
        var availableMechanics = mechanicsList
            .Select(mechanic => new SelectListItem(mechanic.Name, mechanic.Id.ToString()))
            .ToList();
        returnList.AddRange(availableMechanics);
        return returnList;
    }
}