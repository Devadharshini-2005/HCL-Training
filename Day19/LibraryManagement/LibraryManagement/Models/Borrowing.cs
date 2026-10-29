namespace LibraryManagement.Models;

public class Borrowing
{
    public int BorrowingId { get; set; }

    public int BookId { get; set; }

    public int MemberId { get; set; }

    public DateTime BorrowedAt { get; set; } = DateTime.Today;

    public DateTime DueDate { get; set; } = DateTime.Today.AddDays(14);

    public DateTime? ReturnedAt { get; set; }

    public Book? Book { get; set; }

    public Member? Member { get; set; }
}