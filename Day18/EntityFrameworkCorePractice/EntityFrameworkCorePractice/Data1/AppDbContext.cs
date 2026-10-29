
using System;
using EntityFrameworkCorePractice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFrameworkCorePractice.Data1
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Department> Departments => Set<Department>();

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=.\\SQLEXPRESS;" +
                    "Database=EFCoreDB;" +
                    "Trusted_Connection=True;" +
                    "TrustServerCertificate=True;");

                optionsBuilder.LogTo(
                    Console.WriteLine,
                    LogLevel.Warning);
            }
        }
    }
}
