using MovieTicketBooking.DTOs.Shows;

namespace MovieTicketBooking.Services.Interfaces;

public interface IShowService
{
    Task<ShowResponse> CreateShowAsync(CreateShowRequest request);

    Task<List<ShowResponse>> GetAllShowsAsync(
        DateTime? date,
        int? movieId,
        int? theatreId);

    Task<ShowResponse?> GetShowByIdAsync(int showId);

    Task<List<ShowResponse>> GetShowsByMovieIdAsync(int movieId);

    Task<List<ShowResponse>> GetShowsByTheatreIdAsync(int theatreId);

    Task<List<ShowResponse>> SearchShowsByDateAsync(DateTime date);

    Task<ShowResponse?> UpdateShowAsync(
        int showId,
        UpdateShowRequest request);

    Task<bool> DeleteShowAsync(int showId);

    Task<List<ShowSeatResponse>> GetShowSeatsAsync(int showId);

    Task<List<ShowSeatResponse>> GetAvailableShowSeatsAsync(int showId);

    Task<List<ShowSeatResponse>> GetBookedShowSeatsAsync(int showId);
}