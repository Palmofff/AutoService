using AutoService.Models;
using AutoService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Controllers;

public class WorkOrdersController : Controller
{
    private readonly IAutoServiceRepository _repository;

    public WorkOrdersController(IAutoServiceRepository repository)
    {
        _repository = repository;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        return View(await _repository.GetAllOrdersAsync());
    }

    public async Task<IActionResult> Create()
    {
        var model = await _repository.InitializeWorkOrderModel();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(WorkOrderModel workOrderModel)
    {
        if (ModelState.IsValid)
        {
            await _repository.AddNewWorkOrder(workOrderModel);
            return RedirectToAction(nameof(Index));
        }

        return View(workOrderModel);
    }

    public async Task<IActionResult> OrdersWithoutMechanic()
    {
        return View(await _repository.GetOrdersWithoutMechanicAsync());
    }
    
    public async Task<IActionResult> UnfinishedOrders()
    {
        return View(await _repository.UnfinishedOrdersAsync());
    }

    public async Task<IActionResult> OrderWithoutPrice()
    {
        return View(await _repository.GetOrderWithoutPriceAsync());
    }

    public async Task<IActionResult> AppointMechanic(int id)
    {
        if (id == null)
        {
            return View("NotFound");
        }
        var orderModel = await _repository.GetWorkOrderByIdAsync(id);
        if (orderModel == null)
        {
            return View("NotFound");
        }
        await _repository.GetAvailableMechanics(orderModel);

        return View(orderModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AppointMechanic(int id, WorkOrderModel orderModel)
    {
        if (id != orderModel.Id) return NotFound();

        switch (ModelState.IsValid)
        {
            case true:
                await _repository.UpdateWorkOrderAsync(orderModel);
                return RedirectToAction(nameof(Index));
            default:
                return View(orderModel);
        }
    }

    public async Task<IActionResult> FinishOrder(int id)
    {
        {
            if (id == null)
            {
                return View("NotFound");
            }

            var orderModel = await _repository.GetWorkOrderByIdAsync(id);
            return orderModel == null ? View("NotFound") : View(orderModel);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> FinishOrder(int id, WorkOrderModel orderModel)
    {
        if (id != orderModel.Id) return NotFound();
        switch (ModelState.IsValid)
        {
            case true:
                await _repository.FinishOrderAsync(orderModel);
                return RedirectToAction(nameof(Index));
            default: 
                return View(orderModel); 
        }
    }
}