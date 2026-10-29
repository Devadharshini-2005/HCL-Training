using LibraryManagement.Data;
using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

public class BorrowingRepository
{
    public List<Borrowing> GetAllBorrowings()
    {
        using var context = new AppDbContext();

        return context.Borrowings
            .AsNoTracking()
            .Include(b => b.Book)
            .Include(b => b.Member)
            .OrderBy(b => b.BorrowingId)
            .ToList();
    }

    public Borrowing? GetBorrowingById(int borrowingId)
    {
        using var context = new AppDbContext();

        return context.Borrowings
            .AsNoTracking()
            .Include(b => b.Book)
            .Include(b => b.Member)
            .FirstOrDefault(b => b.BorrowingId == borrowingId);
    }

    public void BorrowBook(int bookId, int memberId, DateTime dueDate)
    {
        using var context = new AppDbContext();
        using var transaction = context.Database.BeginTransaction();

        var book = context.Books
            .FirstOrDefault(b => b.BookId == bookId);

        if (book == null)
        {
            throw new KeyNotFoundException("Book not found.");
        }

        var member = context.Members
            .FirstOrDefault(m => m.MemberId == memberId);

        if (member == null)
        {
            throw new KeyNotFoundException("Member not found.");
        }

        if (book.AvailableCopies <= 0)
        {
            throw new InvalidOperationException(
                "No copies of this book are currently available.");
        }

        if (dueDate.Date < DateTime.Today)
        {
            throw new ArgumentException(
                "Due date cannot be in the past.");
        }

        var borrowing = new Borrowing
        {
            BookId = bookId,
            MemberId = memberId,
            BorrowedAt = DateTime.Today,
            DueDate = dueDate.Date,
            ReturnedAt = null
        };

        book.AvailableCopies--;

        context.Borrowings.Add(borrowing);
        context.SaveChanges();
        transaction.Commit();
    }

    public void ReturnBook(int borrowingId)
    {
        using var context = new AppDbContext();
        using var transaction = context.Database.BeginTransaction();

        var borrowing = context.Borrowings
            .FirstOrDefault(b => b.BorrowingId == borrowingId);

        if (borrowing == null)
        {
            throw new KeyNotFoundException(
                "Borrowing record not found.");
        }

        if (borrowing.ReturnedAt != null)
        {
            throw new InvalidOperationException(
                "This book has already been returned.");
        }

        var book = context.Books
            .FirstOrDefault(b => b.BookId == borrowing.BookId);

        if (book == null)
        {
            throw new InvalidOperationException(
                "The associated book was not found.");
        }

        if (book.AvailableCopies >= book.TotalCopies)
        {
            throw new InvalidOperationException(
                "Available copies cannot exceed total copies.");
        }

        borrowing.ReturnedAt = DateTime.Today;
        book.AvailableCopies++;

        context.SaveChanges();
        transaction.Commit();
    }
}