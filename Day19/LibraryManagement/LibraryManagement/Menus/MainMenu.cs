using LibraryManagement.Services;

namespace LibraryManagement.Menus;

public class MainMenu
{
    private readonly BookMenu _bookMenu;
    private readonly MemberMenu _memberMenu;
    private readonly BorrowingMenu _borrowingMenu;
    private readonly AuditLogMenu _auditLogMenu;

    public MainMenu(
    BookMenu bookMenu,
    MemberMenu memberMenu,
    BorrowingMenu borrowingMenu,
    AuditLogMenu auditLogMenu)
    {
        _bookMenu = bookMenu;
        _memberMenu = memberMenu;
        _borrowingMenu = borrowingMenu;
        _auditLogMenu = auditLogMenu;
    }

    public void ShowMenu()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("====================================");
            Console.WriteLine("      LIBRARY MANAGEMENT SYSTEM");
            Console.WriteLine("====================================");
            Console.WriteLine("1. Book Management");
            Console.WriteLine("2. Member Management");
            Console.WriteLine("3. Borrowing and Returning");
            Console.WriteLine("4. View Audit Logs");
            Console.WriteLine("0. Exit");
            Console.WriteLine("====================================");
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    _bookMenu.ShowMenu();
                    break;

                case "2":
                    _memberMenu.ShowMenu();
                    break;

                case "3":
                    _borrowingMenu.ShowMenu();
                    break;

                case "4":
                    _auditLogMenu.ShowMenu();
                    break;

                case "0":
                    Console.WriteLine("Thank you for using the Library Management System.");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }
    }
}