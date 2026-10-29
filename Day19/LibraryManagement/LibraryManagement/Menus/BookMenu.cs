
using LibraryManagement.Models;
using LibraryManagement.Services;

namespace LibraryManagement.Menus;

public class BookMenu
{
    private readonly BookService _bookService;

    public BookMenu(BookService bookService)
    {
        _bookService = bookService;
    }

    public void ShowMenu()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n===== BOOK MANAGEMENT =====");
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. View All Books");
            Console.WriteLine("3. Search Book by ID");
            Console.WriteLine("4. Update Book");
            Console.WriteLine("5. Delete Book");
            Console.WriteLine("0. Back to Main Menu");
            Console.Write("Enter your choice: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        AddBook();
                        break;

                    case "2":
                        ViewAllBooks();
                        break;

                    case "3":
                        SearchBook();
                        break;

                    case "4":
                        UpdateBook();
                        break;

                    case "5":
                        DeleteBook();
                        break;

                    case "0":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid choice. Try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    private void AddBook()
    {
        var book = new Book();

        Console.Write("Enter book title: ");
        book.Title = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter author name: ");
        book.Author = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter ISBN: ");
        book.ISBN = Console.ReadLine() ?? string.Empty;

        book.Price = ReadDecimal("Enter price: ");
        book.TotalCopies = ReadInt("Enter total copies: ");

        book.AvailableCopies = book.TotalCopies;

        _bookService.AddBook(book);

        Console.WriteLine("Book added successfully.");
    }

    private void ViewAllBooks()
    {
        var books = _bookService.GetAllBooks();

        if (books.Count == 0)
        {
            Console.WriteLine("No books found.");
            return;
        }

        Console.WriteLine("\n--- BOOK LIST ---");

        foreach (var book in books)
        {
            DisplayBook(book);
        }
    }

    private void SearchBook()
    {
        int bookId = ReadInt("Enter book ID: ");

        var book = _bookService.GetBookById(bookId);

        DisplayBook(book);
    }

    private void UpdateBook()
    {
        int bookId = ReadInt("Enter book ID to update: ");

        var book = _bookService.GetBookById(bookId);

        Console.WriteLine("Leave a field empty to keep its current value.");

        Console.Write($"Title ({book.Title}): ");
        string? title = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(title))
        {
            book.Title = title;
        }

        Console.Write($"Author ({book.Author}): ");
        string? author = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(author))
        {
            book.Author = author;
        }

        Console.Write($"ISBN ({book.ISBN}): ");
        string? isbn = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(isbn))
        {
            book.ISBN = isbn;
        }

        Console.Write($"Price ({book.Price}): ");
        string? priceInput = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(priceInput))
        {
            if (!decimal.TryParse(priceInput, out decimal price))
            {
                throw new ArgumentException("Invalid price.");
            }

            book.Price = price;
        }

        Console.Write($"Total copies ({book.TotalCopies}): ");
        string? copiesInput = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(copiesInput))
        {
            if (!int.TryParse(copiesInput, out int totalCopies))
            {
                throw new ArgumentException("Invalid total copies.");
            }

            int borrowedCopies =
                book.TotalCopies - book.AvailableCopies;

            if (totalCopies < borrowedCopies)
            {
                throw new ArgumentException(
                    "Total copies cannot be less than the number of borrowed copies.");
            }

            book.TotalCopies = totalCopies;
            book.AvailableCopies = totalCopies - borrowedCopies;
        }

        _bookService.UpdateBook(book);

        Console.WriteLine("Book updated successfully.");
    }

    private void DeleteBook()
    {
        int bookId = ReadInt("Enter book ID to delete: ");

        Console.Write("Are you sure you want to delete this book? (y/n): ");
        string? confirmation = Console.ReadLine();

        if (!string.Equals(
                confirmation,
                "y",
                StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("Deletion cancelled.");
            return;
        }

        _bookService.DeleteBook(bookId);

        Console.WriteLine("Book deleted successfully.");
    }

    private static void DisplayBook(Book book)
    {
        Console.WriteLine("\n-------------------------");
        Console.WriteLine($"Book ID         : {book.BookId}");
        Console.WriteLine($"Title           : {book.Title}");
        Console.WriteLine($"Author          : {book.Author}");
        Console.WriteLine($"ISBN            : {book.ISBN}");
        Console.WriteLine($"Price           : {book.Price:F2}");
        Console.WriteLine($"Total Copies    : {book.TotalCopies}");
        Console.WriteLine($"Available Copies: {book.AvailableCopies}");
        Console.WriteLine("-------------------------");
    }

    private static int ReadInt(string message)
    {
        Console.Write(message);

        if (!int.TryParse(Console.ReadLine(), out int value))
        {
            throw new ArgumentException("Please enter a valid whole number.");
        }

        return value;
    }

    private static decimal ReadDecimal(string message)
    {
        Console.Write(message);

        if (!decimal.TryParse(Console.ReadLine(), out decimal value))
        {
            throw new ArgumentException("Please enter a valid price.");
        }

        return value;
    }
}
