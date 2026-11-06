
namespace CampusPlacementManagement.DTOs.Reports;

public class StudentPlacementReportDto
{
    public int StudentId { get; set; }

    public string StudentName { get; set; } = string.Empty;

    public string RegisterNumber { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public int GraduationYear { get; set; }

    public int TotalApplications { get; set; }

    public int ShortlistedApplications { get; set; }

    public int SelectedApplications { get; set; }

    public bool IsPlaced { get; set; }
}
