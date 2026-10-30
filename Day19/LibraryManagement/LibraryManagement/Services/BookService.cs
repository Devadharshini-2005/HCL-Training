
using LibraryManagement.Models;
using LibraryManagement.Repositories;

namespace LibraryManagement.Services;

public class BookService
{
    private readonly BookRepository _bookRepository;
    private readonly AuditLogService _auditLogService;

    public BookService(
    BookRepository bookRepository,
    AuditLogService auditLogService)
    {
        _bookRepository = bookRepository;
        _auditLogService = auditLogService;
    }
    public void AddBook(Book book)
    {
        ValidateBook(book);

        var existingBooks = _bookRepository.GetAllBooks();

        bool isbnExists = existingBooks.Any(existingBook =>
            existingBook.ISBN.Equals(
                book.ISBN,
                StringComparison.OrdinalIgnoreCase));

        if (isbnExists)
        {
            throw new InvalidOperationException(
                "A book with this ISBN already exists.");
        }

        book.Title = book.Title.Trim();
        book.Author = book.Author.Trim();
        book.ISBN = book.ISBN.Trim();

        _bookRepository.AddBook(book);
        _auditLogService.AddAuditLog(new AuditLog
        {
            Action = "INSERT",
            EntityName = "Book",
            EntityId = book.BookId,
            Description = $"Book '{book.Title}' was added."
        });
    }

    public List<Book> GetAllBooks()
    {
        return _bookRepository.GetAllBooks();
    }

    public Book GetBookById(int bookId)
    {
        if (bookId <= 0)
        {
            throw new ArgumentException(
                "Book ID must be greater than zero.");
        }

        var book = _bookRepository.GetBookById(bookId);

        if (book == null)
        {
            throw new KeyNotFoundException(
                $"Book with ID {bookId} was not found.");
        }

        return book;
    }

    public void UpdateBook(Book book)
    {
        if (book.BookId <= 0)
        {
            throw new ArgumentException(
                "Book ID must be greater than zero.");
        }

        ValidateBook(book);

        book.Title = book.Title.Trim();
        book.Author = book.Author.Trim();
        book.ISBN = book.ISBN.Trim();

        var existingBooks = _bookRepository.GetAllBooks();

        bool duplicateIsbn = existingBooks.Any(existingBook =>
            existingBook.ISBN.Equals(
                book.ISBN,
                StringComparison.OrdinalIgnoreCase)
            && existingBook.BookId != book.BookId);

        if (duplicateIsbn)
        {
            throw new InvalidOperationException(
                "Another book already uses this ISBN.");
        }

        bool updated = _bookRepository.UpdateBook(book);

        if (!updated)
        {
            throw new KeyNotFoundException(
                $"Book with ID {book.BookId} was not found.");
        }
    }

    public void DeleteBook(int bookId)
    {
        if (bookId <= 0)
        {
            throw new ArgumentException(
                "Book ID must be greater than zero.");
        }

        var book = _bookRepository.GetBookById(bookId);

        if (book == null)
        {
            throw new KeyNotFoundException(
                $"Book with ID {bookId} was not found.");
        }

        if (book.AvailableCopies < book.TotalCopies)
        {
            throw new InvalidOperationException(
                "This book has borrowed copies and cannot be deleted.");
        }

        bool deleted = _bookRepository.DeleteBook(bookId);

        if (!deleted)
        {
            throw new InvalidOperationException(
                "Unable to delete the book.");
        }
    }

    private static void ValidateBook(Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        if (string.IsNullOrWhiteSpace(book.Title))
        {
            throw new ArgumentException(
                "Book title cannot be empty.");
        }

        if (book.Title.Trim().Length > 150)
        {
            throw new ArgumentException(
                "Book title cannot exceed 150 characters.");
        }

        if (string.IsNullOrWhiteSpace(book.Author))
        {
            throw new ArgumentException(
                "Author name cannot be empty.");
        }

        if (book.Author.Trim().Length > 100)
        {
            throw new ArgumentException(
                "Author name cannot exceed 100 characters.");
        }

        if (string.IsNullOrWhiteSpace(book.ISBN))
        {
            throw new ArgumentException(
                "ISBN cannot be empty.");
        }

        if (book.ISBN.Trim().Length > 20)
        {
            throw new ArgumentException(
                "ISBN cannot exceed 20 characters.");
        }

        if (book.Price < 0)
        {
            throw new ArgumentException(
                "Book price cannot be negative.");
        }

        if (book.TotalCopies < 0)
        {
            throw new ArgumentException(
                "Total copies cannot be negative.");
        }

        if (book.AvailableCopies < 0)
        {
            throw new ArgumentException(
                "Available copies cannot be negative.");
        }

        if (book.AvailableCopies > book.TotalCopies)
        {
            throw new ArgumentException(
                "Available copies cannot exceed total copies.");
        }
    }
}
