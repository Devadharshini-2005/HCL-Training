using LibraryManagement.Models;
using LibraryManagement.Services;

namespace LibraryManagement.Menus;

public class BorrowingMenu
{
    private readonly BorrowingService _borrowingService;

    public BorrowingMenu(BorrowingService borrowingService)
    {
        _borrowingService = borrowingService;
    }

    public void ShowMenu()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n===== BORROWING MANAGEMENT =====");
            Console.WriteLine("1. Borrow Book");
            Console.WriteLine("2. View All Borrowings");
            Console.WriteLine("3. Search Borrowing by ID");
            Console.WriteLine("4. Return Book");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        BorrowBook();
                        break;

                    case "2":
                        ViewAllBorrowings();
                        break;

                    case "3":
                        SearchBorrowing();
                        break;

                    case "4":
                        ReturnBook();
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

    private void BorrowBook()
    {
        int bookId = ReadInt("Enter book ID: ");
        int memberId = ReadInt("Enter member ID: ");
        DateTime dueDate = ReadDate(
            "Enter due date (yyyy-MM-dd): ");

        _borrowingService.BorrowBook(
            bookId,
            memberId,
            dueDate);

        Console.WriteLine("Book borrowed successfully.");
    }

    private void ViewAllBorrowings()
    {
        List<Borrowing> borrowings =
            _borrowingService.GetAllBorrowings();

        if (borrowings.Count == 0)
        {
            Console.WriteLine("No borrowing records found.");
            return;
        }

        foreach (var borrowing in borrowings)
        {
            DisplayBorrowing(borrowing);
        }
    }

    private void SearchBorrowing()
    {
        int borrowingId = ReadInt("Enter borrowing ID: ");

        var borrowing =
            _borrowingService.GetBorrowingById(borrowingId);

        DisplayBorrowing(borrowing);
    }

    private void ReturnBook()
    {
        int borrowingId =
            ReadInt("Enter borrowing ID to return: ");

        var borrowing =
            _borrowingService.GetBorrowingById(borrowingId);

        if (borrowing.ReturnedAt != null)
        {
            Console.WriteLine("This book has already been returned.");
            return;
        }

        Console.WriteLine(
            $"Book: {borrowing.Book?.Title ?? "Unknown"}");

        Console.WriteLine(
            $"Member: {borrowing.Member?.Name ?? "Unknown"}");

        Console.Write("Confirm return? (y/n): ");

        if (!string.Equals(
                Console.ReadLine(),
                "y",
                StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Return cancelled.");
            return;
        }

        _borrowingService.ReturnBook(borrowingId);

        Console.WriteLine("Book returned successfully.");
    }

    private static void DisplayBorrowing(Borrowing borrowing)
    {
        Console.WriteLine("\n-------------------------");
        Console.WriteLine($"Borrowing ID : {borrowing.BorrowingId}");
        Console.WriteLine($"Book ID      : {borrowing.BookId}");
        Console.WriteLine($"Book Title   : {borrowing.Book?.Title ?? "Unknown"}");
        Console.WriteLine($"Member ID    : {borrowing.MemberId}");
        Console.WriteLine($"Member Name  : {borrowing.Member?.Name ?? "Unknown"}");
        Console.WriteLine($"Borrowed At  : {borrowing.BorrowedAt:yyyy-MM-dd}");
        Console.WriteLine($"Due Date     : {borrowing.DueDate:yyyy-MM-dd}");

        Console.WriteLine(
            $"Returned At  : {(borrowing.ReturnedAt.HasValue ? borrowing.ReturnedAt.Value.ToString("yyyy-MM-dd") : "Not returned")}");

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

    private static DateTime ReadDate(string message)
    {
        Console.Write(message);

        if (!DateTime.TryParse(
                Console.ReadLine(),
                out DateTime date))
        {
            throw new ArgumentException(
                "Invalid date. Use yyyy-MM-dd.");
        }

        return date.Date;
    }
}