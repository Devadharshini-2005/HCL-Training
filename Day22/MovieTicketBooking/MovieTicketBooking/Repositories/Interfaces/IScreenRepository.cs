using MovieTicketBooking.Models;

namespace MovieTicketBooking.Repositories.Interfaces;

public interface IScreenRepository
{
    Task<Screen> AddAsync(Screen screen);

    Task<List<Screen>> GetAllAsync(int? theatreId);

    Task<Screen?> GetByIdAsync(int screenId);

    Task<List<Screen>> GetByTheatreIdAsync(int theatreId);

    Task UpdateAsync(Screen screen);

    Task DeleteAsync(Screen screen);

    Task<bool> HasSeatsAsync(int screenId);

    Task<bool> HasShowsAsync(int screenId);

    Task<bool> TheatreExistsAsync(int theatreId);
}