
using Microsoft.AspNetCore.Identity;
using CampusPlacementManagement.Models.Enums;

namespace CampusPlacementManagement.Models.Identity;

public class ApplicationUser : IdentityUser
{
    // Account status of the user
    public AccountStatus AccountStatus { get; set; }
        = AccountStatus.Pending;

    // User creation date
    public DateTime CreatedAt { get; set; }
        = DateTime.UtcNow;

    // Last updated date
    public DateTime? UpdatedAt { get; set; }
}
