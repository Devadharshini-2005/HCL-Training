
using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

public class BookRepository
{
    private readonly AppDbContext _context;

    public BookRepository(AppDbContext context)
    {
        _context = context;
    }

    public void AddBook(Book book)
    {
        using var context = new AppDbContext();

        context.Books.Add(book);
        context.SaveChanges();
    }

    public List<Book> GetAllBooks()
    {
        using var context = new AppDbContext();

        return context.Books
            .AsNoTracking()
            .OrderBy(book => book.BookId)
            .ToList();
    }

    public Book? GetBookById(int bookId)
    {
        using var context = new AppDbContext();

        return context.Books
            .AsNoTracking()
            .FirstOrDefault(book => book.BookId == bookId);
    }

    public bool UpdateBook(Book updatedBook)
    {
        using var context = new AppDbContext();

        var existingBook = context.Books
            .FirstOrDefault(book => book.BookId == updatedBook.BookId);

        if (existingBook == null)
        {
            return false;
        }

        existingBook.Title = updatedBook.Title;
        existingBook.Author = updatedBook.Author;
        existingBook.ISBN = updatedBook.ISBN;
        existingBook.Price = updatedBook.Price;
        existingBook.TotalCopies = updatedBook.TotalCopies;
        existingBook.AvailableCopies = updatedBook.AvailableCopies;

        context.SaveChanges();

        return true;
    }

    public bool DeleteBook(int bookId)
    {
        using var context = new AppDbContext();

        var book = context.Books
            .FirstOrDefault(book => book.BookId == bookId);

        if (book == null)
        {
            return false;
        }

        context.Books.Remove(book);
        context.SaveChanges();

        return true;
    }
}
