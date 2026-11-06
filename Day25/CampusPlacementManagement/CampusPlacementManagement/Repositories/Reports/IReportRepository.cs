
using CampusPlacementManagement.DTOs.Reports;

namespace CampusPlacementManagement.Repositories.Reports;

public interface IReportRepository
{
    Task<PlatformOverviewReportDto> GetPlatformOverviewAsync();

    Task<CollegeOverviewReportDto> GetCollegeOverviewAsync(int collegeId);

    Task<CompanyRecruitmentReportDto> GetCompanyRecruitmentAsync(int companyId);

    Task<List<StudentPlacementReportDto>> GetStudentPlacementAsync(int collegeId);
}
