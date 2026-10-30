namespace TrainingManagementSystem.Api.Models;

public record LoginRequest(string Email, string Password, string Role);
public record RegisterRequest(string Name, string Email, string Password, string ContactNumber);
public record CreateCourseRequest(string CourseCode, string Title, decimal Fee, int DurationDays, int TotalSeats);
public record CreateEnrollmentRequest(int LearnerId, string CourseCode, bool EarlyBird, int GroupSize, string SessionType);
public record PaymentRequest(int EnrollmentId, string Method);
