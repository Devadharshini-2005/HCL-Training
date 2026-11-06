
using CampusPlacementManagement.DTOs.Reports;
using CampusPlacementManagement.Repositories.Reports;
using CampusPlacementManagement.Services.Interfaces;

namespace CampusPlacementManagement.Services.Implementations;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;

    public ReportService(IReportRepository reportRepository)
    {
        _reportRepository = reportRepository;
    }

    public async Task<PlatformOverviewReportDto> GetPlatformOverviewAsync()
    {
        return await _reportRepository.GetPlatformOverviewAsync();
    }

    public async Task<CollegeOverviewReportDto> GetCollegeOverviewAsync(int collegeId)
    {
        return await _reportRepository.GetCollegeOverviewAsync(collegeId);
    }

    public async Task<CompanyRecruitmentReportDto> GetCompanyRecruitmentAsync(int companyId)
    {
        return await _reportRepository.GetCompanyRecruitmentAsync(companyId);
    }

    public async Task<List<StudentPlacementReportDto>> GetStudentPlacementAsync(int collegeId)
    {
        return await _reportRepository.GetStudentPlacementAsync(collegeId);
    }
}
