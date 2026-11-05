using MovieTicketBooking.DTOs.Shows;
using MovieTicketBooking.Exceptions;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class ShowService : IShowService
{
    private readonly IShowRepository _showRepository;

    public ShowService(IShowRepository showRepository)
    {
        _showRepository = showRepository;
    }

    // Create a new show
    public async Task<ShowResponse> CreateShowAsync(
        CreateShowRequest request)
    {
        if (request.EndTime <= request.StartTime)
        {
            throw new BusinessRuleException(
                "End time must be later than start time.");
        }

        if (request.TicketPrice < 0)
        {
            throw new BusinessRuleException(
                "Ticket price cannot be negative.");
        }

        if (!await _showRepository.MovieExistsAsync(request.MovieId))
        {
            throw new NotFoundException("Movie not found.");
        }

        if (!await _showRepository.ScreenExistsAsync(request.ScreenId))
        {
            throw new NotFoundException("Screen not found.");
        }

        bool hasOverlap =
            await _showRepository.HasOverlappingShowAsync(
                request.ScreenId,
                request.StartTime,
                request.EndTime);

        if (hasOverlap)
        {
            throw new BusinessRuleException(
                "This screen already has a show scheduled during this time.");
        }

        var seats = await _showRepository.GetSeatsByScreenIdAsync(
            request.ScreenId);

        if (seats.Count == 0)
        {
            throw new BusinessRuleException(
                "No seats are configured for this screen.");
        }

        var show = new Show
        {
            MovieId = request.MovieId,
            ScreenId = request.ScreenId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            TicketPrice = request.TicketPrice
        };

        var createdShow = await _showRepository.AddShowWithSeatsAsync(
    show,
    seats);

        return MapToResponse(createdShow);
    }

    // Get all shows with optional filters
    public async Task<List<ShowResponse>> GetAllShowsAsync(
        DateTime? date,
        int? movieId,
        int? theatreId)
    {
        var shows = await _showRepository.GetAllAsync(
            date, movieId, theatreId);

        return shows.Select(MapToResponse).ToList();
    }

    // Get show by ID
    public async Task<ShowResponse?> GetShowByIdAsync(int showId)
    {
        var show = await _showRepository.GetByIdAsync(showId);

        return show == null ? null : MapToResponse(show);
    }

    // Get shows by movie
    public async Task<List<ShowResponse>> GetShowsByMovieIdAsync(
        int movieId)
    {
        var shows = await _showRepository.GetByMovieIdAsync(movieId);

        return shows.Select(MapToResponse).ToList();
    }

    // Get shows by theatre
    public async Task<List<ShowResponse>> GetShowsByTheatreIdAsync(
        int theatreId)
    {
        var shows = await _showRepository.GetByTheatreIdAsync(theatreId);

        return shows.Select(MapToResponse).ToList();
    }

    // Search shows by date
    public async Task<List<ShowResponse>> SearchShowsByDateAsync(
        DateTime date)
    {
        var shows = await _showRepository.SearchByDateAsync(date);

        return shows.Select(MapToResponse).ToList();
    }

    // Update show
    public async Task<ShowResponse?> UpdateShowAsync(
        int showId,
        UpdateShowRequest request)
    {
        var show = await _showRepository.GetByIdAsync(showId);

        if (show == null)
        {
            return null;
        }

        if (request.EndTime <= request.StartTime)
        {
            throw new BusinessRuleException(
                "End time must be later than start time.");
        }

        if (request.TicketPrice < 0)
        {
            throw new BusinessRuleException(
                "Ticket price cannot be negative.");
        }

        if (!await _showRepository.MovieExistsAsync(request.MovieId))
        {
            throw new NotFoundException("Movie not found.");
        }

        if (!await _showRepository.ScreenExistsAsync(request.ScreenId))
        {
            throw new NotFoundException("Screen not found.");
        }

        bool hasOverlap =
            await _showRepository.HasOverlappingShowAsync(
                request.ScreenId,
                request.StartTime,
                request.EndTime,
                showId);

        if (hasOverlap)
        {
            throw new BusinessRuleException(
    "The selected screen already has a show scheduled during this time.");

        }

        show.MovieId = request.MovieId;
        show.ScreenId = request.ScreenId;
        show.StartTime = request.StartTime;
        show.EndTime = request.EndTime;
        show.TicketPrice = request.TicketPrice;

        await _showRepository.UpdateAsync(show);

        return MapToResponse(show);
    }

    // Delete show
    public async Task<bool> DeleteShowAsync(int showId)
    {
        var show = await _showRepository.GetByIdAsync(showId);

        if (show == null)
        {
            return false;
        }

        if (await _showRepository.HasBookingsAsync(showId))
        {
            throw new BusinessRuleException(
    "Cannot delete this show because bookings are associated with it.");

        }

        await _showRepository.DeleteAsync(show);

        return true;
    }

    // Map entity to response DTO
    private static ShowResponse MapToResponse(Show show)
    {
        return new ShowResponse
        {
            ShowId = show.ShowId,
            MovieId = show.MovieId,
            ScreenId = show.ScreenId,
            StartTime = show.StartTime,
            EndTime = show.EndTime,
            TicketPrice = show.TicketPrice
        };
    }

    // Get all seats for a show
    public async Task<List<ShowSeatResponse>> GetShowSeatsAsync(
        int showId)
    {
        if (!await _showRepository.ShowExistsAsync(showId))
        {
            throw new NotFoundException("Show not found.");
        }

        var showSeats = await _showRepository.GetShowSeatsAsync(showId);

        return showSeats.Select(MapShowSeatToResponse).ToList();
    }

    // Get available seats for a show
    public async Task<List<ShowSeatResponse>> GetAvailableShowSeatsAsync(
        int showId)
    {
        if (!await _showRepository.ShowExistsAsync(showId))
        {
            throw new NotFoundException("Show not found.");
        }

        var showSeats =
            await _showRepository.GetShowSeatsByStatusAsync(
                showId,
                ShowSeatStatus.Available);

        return showSeats.Select(MapShowSeatToResponse).ToList();
    }

    // Get booked seats for a show
    public async Task<List<ShowSeatResponse>> GetBookedShowSeatsAsync(
        int showId)
    {
        if (!await _showRepository.ShowExistsAsync(showId))
        {
            throw new NotFoundException("Show not found.");
        }

        var showSeats =
            await _showRepository.GetShowSeatsByStatusAsync(
                showId,
                ShowSeatStatus.Booked);

        return showSeats.Select(MapShowSeatToResponse).ToList();
    }

    // Map ShowSeat entity to response DTO
    private static ShowSeatResponse MapShowSeatToResponse(
        ShowSeat showSeat)
    {
        return new ShowSeatResponse
        {
            ShowSeatId = showSeat.ShowSeatId,
            ShowId = showSeat.ShowId,
            SeatId = showSeat.SeatId,
            RowName = showSeat.Seat.RowName,
            SeatNumber = showSeat.Seat.SeatNumber,
            SeatType = showSeat.Seat.SeatType,
            Status = showSeat.Status.ToString()
        };
    }
}