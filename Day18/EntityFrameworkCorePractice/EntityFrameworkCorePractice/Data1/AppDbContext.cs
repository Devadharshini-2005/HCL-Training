using System;
using System.Collections.Generic;
using System.Text;
using EntityFrameworkCorePractice.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCorePractice.Data1
{
    internal class AppDbContext : DbContext
    {
        public DbSet<Department> Departments => Set<Department>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=EFCoreDB;Trusted_Connection=True;");
                optionsBuilder.LogTo(Console.WriteLine);
            }
        }

    }
}
