using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IShowRepository
{
    Task<Show> AddAsync(Show show);

    Task<List<Show>> GetAllAsync(
        DateTime? date,
        int? movieId,
        int? theatreId);

    Task<Show?> GetByIdAsync(int showId);

    Task<List<Show>> GetByMovieIdAsync(int movieId);

    Task<List<Show>> GetByTheatreIdAsync(int theatreId);

    Task<List<Show>> SearchByDateAsync(DateTime date);

    Task UpdateAsync(Show show);

    Task DeleteAsync(Show show);

    Task<bool> MovieExistsAsync(int movieId);

    Task<bool> ScreenExistsAsync(int screenId);

    Task<bool> HasBookingsAsync(int showId);

    Task<bool> HasOverlappingShowAsync(
        int screenId,
        DateTime startTime,
        DateTime endTime,
        int? excludeShowId = null);

    Task<List<Seat>> GetSeatsByScreenIdAsync(int screenId);

    Task AddShowSeatsAsync(List<ShowSeat> showSeats);

    Task<List<ShowSeat>> GetShowSeatsAsync(int showId);

    Task<List<ShowSeat>> GetShowSeatsByStatusAsync(
        int showId,
        ShowSeatStatus status);

    Task<bool> ShowExistsAsync(int showId);

    Task<Show> AddShowWithSeatsAsync(Show show, List<Seat> seats);

}