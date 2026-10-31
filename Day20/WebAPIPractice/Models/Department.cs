namespace WebApiPractice.Models;

public class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    // Navigation property
    public ICollection<Student> Students { get; set; }
        = new List<Student>();
}