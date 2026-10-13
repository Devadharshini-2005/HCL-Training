
using ComplaintManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ComplaintManagementAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Complaint> Complaints { get; set; }
    }
}