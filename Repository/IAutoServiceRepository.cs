using AutoService.Data;
using AutoService.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoService.Repository;

public interface IAutoServiceRepository
{
    Task AddNewMechanic(Mechanic mechanic);
    Task<List<WorkOrderModel?>> GetAllOrdersAsync();
    Task<List<WorkOrderModel?>> GetOrdersWithoutMechanicAsync();
    Task<List<WorkOrderModel?>> GetOrderWithoutPriceAsync();
    Task<List<WorkOrderModel?>> UnfinishedOrdersAsync();
    Task<WorkOrderModel?> GetWorkOrderByIdAsync(int id);
    Task<WorkOrder?> GetWorkOrderById(int id);
    Task UpdateWorkOrderAsync(WorkOrderModel workOrderModel);
    Task FinishOrderAsync(WorkOrderModel workOrderModel);
    Task AddNewWorkOrder(WorkOrderModel workOrderModel);
    Task<Customer> CheckForCustomerAsync(WorkOrderModel workOrderModel);
    Task<WorkOrderModel> InitializeWorkOrderModel();
    Task<Car> CheckForCarAsync(WorkOrderModel workOrderModel);
    Task GetAvailableMechanics(WorkOrderModel workOrderModel);
    Task<Car?> GetCarAsync(string plateNumber);
    Task<Customer?> GetCustomerAsync(int id);
    Task<Mechanic?> GetMechanicAsync(int? id);
    Task EditMechanicAsync(Mechanic mechanic);
    Task<List<Mechanic>> GetAllMechanicsAsync();
    Task<List<AnnualReportModel>> GetReportAsync();
}