
using CampusPlacementManagement.DTOs.Reports;

namespace CampusPlacementManagement.Services.Interfaces;

public interface IReportService
{
    Task<PlatformOverviewReportDto> GetPlatformOverviewAsync();

    Task<CollegeOverviewReportDto> GetCollegeOverviewAsync(int collegeId);

    Task<CompanyRecruitmentReportDto> GetCompanyRecruitmentAsync(int companyId);

    Task<List<StudentPlacementReportDto>> GetStudentPlacementAsync(int collegeId);
}
