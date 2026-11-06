
namespace CampusPlacementManagement.DTOs.Reports;

public class CompanyRecruitmentReportDto
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = string.Empty;

    public int TotalJobsPosted { get; set; }

    public int PublishedJobs { get; set; }

    public int ClosedJobs { get; set; }

    public int TotalApplicationsReceived { get; set; }

    public int TotalStudentsSelected { get; set; }

    public int TotalRecruitmentDrives { get; set; }

    public int TotalInterviewsScheduled { get; set; }
}
