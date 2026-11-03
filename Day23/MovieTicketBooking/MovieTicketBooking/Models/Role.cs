using System.ComponentModel.DataAnnotations;

namespace MovieTicketBooking.Models;

public class Role
{
    public int RoleId { get; set; }

    [Required]
    [MaxLength(50)]
    public string RoleName { get; set; } = string.Empty;

    public ICollection<User> Users { get; set; } = new List<User>();
}