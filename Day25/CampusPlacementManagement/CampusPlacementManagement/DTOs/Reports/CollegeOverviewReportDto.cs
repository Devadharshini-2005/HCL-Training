
namespace CampusPlacementManagement.DTOs.Reports;

public class CollegeOverviewReportDto
{
    public int CollegeId { get; set; }

    public string CollegeName { get; set; } = string.Empty;

    public int TotalStudents { get; set; }

    public int ApprovedStudents { get; set; }

    public int PendingStudents { get; set; }

    public int TotalApplications { get; set; }

    public int SelectedStudents { get; set; }

    public int TotalPlacementOfficers { get; set; }

    public int TotalJobsAvailable { get; set; }
}
