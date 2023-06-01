using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AutoService.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoService.Models;

public class WorkOrderModel
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    [DisplayName("Customer")]
    public string CustomerName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string CarPlateNumber { get; set; } = null!;
    [DisplayName("Car")]
    public string CarModel { get; set; } = null!;
    [DisplayName("Year of manufacture")]
    public int? CarYear { get; set; }
    public int? MechanicId { get; set; }
    [DisplayName("Mechanic")]
    public string? MechanicName { get; set; }
    
    public List<SelectListItem>? AvailableMechanics { get; set; } = new();
    [Required]
    public string Description { get; set; } = null!;
    [DataType(DataType.Currency)]
    public int? Price { get; set; }
    [Required]
    [DisplayName("Date of work starting")]
    public DateTime StartDate { get; set; }
    [DisplayName("Date of work ending")]
    public DateTime? EndDate { get; set; }
    public int? Discount { get; set; }
    [Required]
    [DisplayName("Re-repair")]
    public bool ReRepair { get; set; }

    public static WorkOrderModel? FromWorkOrder(WorkOrder? workOrder)
    {
        return new WorkOrderModel()
        {
            Id = workOrder.Id,
            CustomerId = workOrder.Customer.Id,
            CustomerName = workOrder.Customer.Name,
            PhoneNumber = workOrder.Customer.PhoneNumber,
            CarPlateNumber = workOrder.CarPlateNumber ,
            CarYear = workOrder.Car.YearOfManufacture,
            CarModel = workOrder.Car.Model,
            MechanicId = workOrder.MechanicId ?? 0,
            MechanicName = workOrder.Mechanic?.Name,
            Description = workOrder.Description,
            Price = workOrder.Price,
            StartDate = workOrder.StartDate,
            EndDate = workOrder.EndDate,
            Discount = workOrder.Discount,
            ReRepair = workOrder.ReRepair
        };
    }

    public WorkOrder ToWorkOrder()
    {
        return new WorkOrder()
        {
            Id = Id,
            CustomerId = CustomerId,
            CarPlateNumber = CarPlateNumber,
            MechanicId = MechanicId,
            Description = Description,
            Price = Price,
            StartDate = StartDate,
            EndDate = EndDate,
            Discount = Discount,
            ReRepair = ReRepair
        };
    }
}