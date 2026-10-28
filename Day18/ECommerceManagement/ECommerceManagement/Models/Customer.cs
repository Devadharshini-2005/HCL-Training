namespace ECommerceManagement.Models;

public class Customer
{
    public int CustomerId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; } 
    public bool IsActive { get; set; }

    public Customer(string fullName, string email,string phone, string address)
    {
        CustomerId++;
        FullName = fullName;
        Email = email;
        Phone = phone;
        Address = address;
        CreatedAt = DateTime.Now;
        IsActive = true;
    }
    public Customer(){ }

}