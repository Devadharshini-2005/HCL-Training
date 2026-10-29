using EntityFrameworkCorePractice.Data1;
using EntityFrameworkCorePractice.Models;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main()
    {
        using AppDbContext db = new AppDbContext();

        while (true)
        {
            Console.WriteLine("\n===== Department Management =====");
            Console.WriteLine("1. Create Department");
            Console.WriteLine("2. View All Departments");
            Console.WriteLine("3. Find Department By ID");
            Console.WriteLine("4. Update Department");
            Console.WriteLine("5. Delete Department");
            Console.WriteLine("6. Exit");
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateDepartment(db);
                    break;

                case "2":
                    ReadAllDepartments(db);
                    break;

                case "3":
                    ReadDepartmentById(db);
                    break;

                case "4":
                    UpdateDepartment(db);
                    break;

                case "5":
                    DeleteDepartment(db);
                    break;

                case "6":
                    return;

                default:
                    Console.WriteLine("Invalid choice. Try again.");
                    break;
            }
        }
    }

    // CREATE
    static void CreateDepartment(AppDbContext db)
    {
        Department department = new Department();

        Console.Write("Enter department name: ");
        department.DepartmentName = Console.ReadLine();

        Console.Write("Enter location: ");
        department.Location = Console.ReadLine();

        Console.Write("Is active? (true/false): ");
        if (!bool.TryParse(Console.ReadLine(), out bool isActive))
        {
            Console.WriteLine("Invalid active status.");
            return;
        }

        department.IsActive = isActive;

        db.Departments.Add(department);
        db.SaveChanges();

        Console.WriteLine(
            $"Department created successfully. ID: {department.DepartmentId}");
    }

    // READ ALL
    static void ReadAllDepartments(AppDbContext db)
    {
        List<Department> departments = db.Departments
            .AsNoTracking()
            .ToList();

        if (departments.Count == 0)
        {
            Console.WriteLine("No departments found.");
            return;
        }

        foreach (Department department in departments)
        {
            Console.WriteLine(
                $"ID: {department.DepartmentId}, " +
                $"Name: {department.DepartmentName}, " +
                $"Location: {department.Location}, " +
                $"Active: {department.IsActive}");
        }
    }

    // READ BY ID
    static void ReadDepartmentById(AppDbContext db)
    {
        Console.Write("Enter department ID: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid department ID.");
            return;
        }

        Department? department = db.Departments
            .AsNoTracking()
            .FirstOrDefault(d => d.DepartmentId == id);

        if (department == null)
        {
            Console.WriteLine("Department not found.");
            return;
        }

        Console.WriteLine($"ID: {department.DepartmentId}");
        Console.WriteLine($"Name: {department.DepartmentName}");
        Console.WriteLine($"Location: {department.Location}");
        Console.WriteLine($"Active: {department.IsActive}");
    }

    // UPDATE
    static void UpdateDepartment(AppDbContext db)
    {
        Console.Write("Enter department ID to update: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid department ID.");
            return;
        }

        Department? department = db.Departments
            .FirstOrDefault(d => d.DepartmentId == id);

        if (department == null)
        {
            Console.WriteLine("Department not found.");
            return;
        }

        Console.Write("Enter new department name: ");
        department.DepartmentName = Console.ReadLine();

        Console.Write("Enter new location: ");
        department.Location = Console.ReadLine();

        Console.Write("Is active? (true/false): ");
        if (!bool.TryParse(Console.ReadLine(), out bool isActive))
        {
            Console.WriteLine("Invalid active status. Update cancelled.");
            return;
        }

        department.IsActive = isActive;

        db.SaveChanges();

        Console.WriteLine("Department updated successfully.");
    }

    // DELETE
    static void DeleteDepartment(AppDbContext db)
    {
        Console.Write("Enter department ID to delete: ");

        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("Invalid department ID.");
            return;
        }

        Department? department = db.Departments
            .FirstOrDefault(d => d.DepartmentId == id);

        if (department == null)
        {
            Console.WriteLine("Department not found.");
            return;
        }

        db.Departments.Remove(department);
        db.SaveChanges();

        Console.WriteLine("Department deleted successfully.");
    }
}
