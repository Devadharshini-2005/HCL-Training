using ProductManagementMVC.ViewModels;

namespace ProductManagementMVC.Repositories.Interfaces;

public interface IDashboardRepository
{
    Task<DashboardViewModel> GetStatisticsAsync();
}
