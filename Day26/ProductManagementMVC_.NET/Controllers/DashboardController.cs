using Microsoft.AspNetCore.Mvc;
using ProductManagementMVC.Services.Interfaces;

namespace ProductManagementMVC.Controllers;

public class DashboardController : Controller
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var dashboard = await _dashboardService.GetStatisticsAsync();
        return View(dashboard);
    }
}
