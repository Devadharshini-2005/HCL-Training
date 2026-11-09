using ProductManagementMVC.ViewModels;

namespace ProductManagementMVC.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardViewModel> GetStatisticsAsync();
}
