using LibraryManagement.Models;
using LibraryManagement.Repositories;

namespace LibraryManagement.Services;

public class BorrowingService
{
    private readonly BorrowingRepository _borrowingRepository;

    public BorrowingService()
    {
        _borrowingRepository = new BorrowingRepository();
    }

    public List<Borrowing> GetAllBorrowings()
    {
        return _borrowingRepository.GetAllBorrowings();
    }

    public Borrowing GetBorrowingById(int borrowingId)
    {
        if (borrowingId <= 0)
        {
            throw new ArgumentException(
                "Borrowing ID must be greater than zero.");
        }

        var borrowing =
            _borrowingRepository.GetBorrowingById(borrowingId);

        if (borrowing == null)
        {
            throw new KeyNotFoundException(
                $"Borrowing record {borrowingId} was not found.");
        }

        return borrowing;
    }

    public void BorrowBook(int bookId, int memberId, DateTime dueDate)
    {
        if (bookId <= 0)
        {
            throw new ArgumentException(
                "Book ID must be greater than zero.");
        }

        if (memberId <= 0)
        {
            throw new ArgumentException(
                "Member ID must be greater than zero.");
        }

        if (dueDate.Date < DateTime.Today)
        {
            throw new ArgumentException(
                "Due date cannot be in the past.");
        }

        _borrowingRepository.BorrowBook(
            bookId,
            memberId,
            dueDate.Date);
    }

    public void ReturnBook(int borrowingId)
    {
        if (borrowingId <= 0)
        {
            throw new ArgumentException(
                "Borrowing ID must be greater than zero.");
        }

        _borrowingRepository.ReturnBook(borrowingId);
    }
}