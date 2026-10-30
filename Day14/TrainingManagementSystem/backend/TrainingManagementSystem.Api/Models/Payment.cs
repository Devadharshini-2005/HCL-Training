namespace TrainingManagementSystem.Api.Models;

public class Payment
{
    public int Id { get; set; }
    public int EnrollmentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.Now;
    public string Status { get; set; } = "Pending";
    public string Method { get; set; } = "Online";
}
