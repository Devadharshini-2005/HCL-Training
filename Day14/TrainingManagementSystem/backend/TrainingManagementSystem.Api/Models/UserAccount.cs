using System.ComponentModel.DataAnnotations;

namespace TrainingManagementSystem.Api.Models;

public class UserAccount
{
    [Key] public int Id { get; set; }
    [Required, MaxLength(100)] public string Name { get; set; } = "";
    [Required, EmailAddress, MaxLength(180)] public string Email { get; set; } = "";
    [Required, MaxLength(100)] public string Password { get; set; } = "";
    [Required, MaxLength(20)] public string Role { get; set; } = "Student";
    [MaxLength(20)] public string ContactNumber { get; set; } = "";
}
