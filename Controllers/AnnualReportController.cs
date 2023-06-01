using AutoService.Models;
using AutoService.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AutoService.Controllers;

public class AnnualReportController : Controller
{
    private readonly IAutoServiceRepository _repository;

    public AnnualReportController(IAutoServiceRepository repository)
    {
        _repository = repository;
    }
    // GET
    public async Task<IActionResult> Index()
    {
        return View(await _repository.GetReportAsync());
    }

    
}