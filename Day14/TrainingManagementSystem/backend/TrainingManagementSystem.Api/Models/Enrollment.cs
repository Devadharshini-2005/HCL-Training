namespace TrainingManagementSystem.Api.Models;

public class Enrollment
{
    public int Id { get; set; }
    public int LearnerId { get; set; }
    public string LearnerName { get; set; } = "";
    public string CourseCode { get; set; } = "";
    public string CourseTitle { get; set; } = "";
    public DateTime EnrollmentDate { get; set; } = DateTime.Now;
    public decimal BaseFee { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal FinalFee { get; set; }
    public string Status { get; set; } = "Active";
    public string PaymentStatus { get; set; } = "Pending";
}
