using ProductManagementMVC.Repositories.Interfaces;
using ProductManagementMVC.Services.Interfaces;
using ProductManagementMVC.ViewModels;

namespace ProductManagementMVC.Services;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardService(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public Task<DashboardViewModel> GetStatisticsAsync()
    {
        return _dashboardRepository.GetStatisticsAsync();
    }
}
