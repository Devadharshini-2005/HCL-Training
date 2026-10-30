using System.ComponentModel.DataAnnotations;

namespace TrainingManagementSystem.Api.Models;

public class Course
{
    [Key] public int Id { get; set; }
    [Required, MaxLength(20)] public string CourseCode { get; set; } = "";
    [Required, MaxLength(120)] public string Title { get; set; } = "";
    public decimal Fee { get; set; }
    public int DurationDays { get; set; }
    public int TotalSeats { get; set; }
    public int EnrolledCount { get; set; }
    public int AvailableSeats => Math.Max(0, TotalSeats - EnrolledCount);
}
