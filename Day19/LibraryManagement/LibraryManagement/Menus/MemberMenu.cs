
using LibraryManagement.Models;
using LibraryManagement.Services;

namespace LibraryManagement.Menus;

public class MemberMenu
{
    private readonly MemberService _memberService;

    public MemberMenu()
    {
        _memberService = new MemberService();
    }

    public void ShowMenu()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n===== MEMBER MANAGEMENT =====");
            Console.WriteLine("1. Add Member");
            Console.WriteLine("2. View All Members");
            Console.WriteLine("3. Search Member by ID");
            Console.WriteLine("4. Update Member");
            Console.WriteLine("5. Delete Member");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        AddMember();
                        break;
                    case "2":
                        ViewAllMembers();
                        break;
                    case "3":
                        SearchMember();
                        break;
                    case "4":
                        UpdateMember();
                        break;
                    case "5":
                        DeleteMember();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private void AddMember()
    {
        var member = new Member();

        Console.Write("Enter member name: ");
        member.Name = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter email: ");
        member.Email = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter phone number: ");
        member.PhoneNumber = Console.ReadLine() ?? string.Empty;

        member.MembershipDate = DateTime.Today;

        _memberService.AddMember(member);

        Console.WriteLine("Member added successfully.");
    }

    private void ViewAllMembers()
    {
        var members = _memberService.GetAllMembers();

        if (members.Count == 0)
        {
            Console.WriteLine("No members found.");
            return;
        }

        foreach (var member in members)
        {
            DisplayMember(member);
        }
    }

    private void SearchMember()
    {
        int memberId = ReadInt("Enter member ID: ");

        var member = _memberService.GetMemberById(memberId);

        DisplayMember(member);
    }

    private void UpdateMember()
    {
        int memberId = ReadInt("Enter member ID to update: ");

        var member = _memberService.GetMemberById(memberId);

        Console.WriteLine("Leave a field empty to keep its current value.");

        Console.Write($"Name ({member.Name}): ");
        string? name = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(name))
        {
            member.Name = name;
        }

        Console.Write($"Email ({member.Email}): ");
        string? email = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(email))
        {
            member.Email = email;
        }

        Console.Write($"Phone ({member.PhoneNumber}): ");
        string? phone = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(phone))
        {
            member.PhoneNumber = phone;
        }

        _memberService.UpdateMember(member);

        Console.WriteLine("Member updated successfully.");
    }

    private void DeleteMember()
    {
        int memberId = ReadInt("Enter member ID to delete: ");

        Console.Write("Confirm deletion? (y/n): ");

        if (!string.Equals(
                Console.ReadLine(),
                "y",
                StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Deletion cancelled.");
            return;
        }

        _memberService.DeleteMember(memberId);

        Console.WriteLine("Member deleted successfully.");
    }

    private static void DisplayMember(Member member)
    {
        Console.WriteLine("\n-------------------------");
        Console.WriteLine($"Member ID      : {member.MemberId}");
        Console.WriteLine($"Name           : {member.Name}");
        Console.WriteLine($"Email          : {member.Email}");
        Console.WriteLine($"Phone          : {member.PhoneNumber}");
        Console.WriteLine($"Membership Date: {member.MembershipDate:yyyy-MM-dd}");
        Console.WriteLine("-------------------------");
    }

    private static int ReadInt(string message)
    {
        Console.Write(message);

        if (!int.TryParse(Console.ReadLine(), out int value))
        {
            throw new ArgumentException(
                "Please enter a valid whole number.");
        }

        return value;
    }
}
