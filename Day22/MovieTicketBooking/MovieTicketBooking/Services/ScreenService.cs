using MovieTicketBooking.DTOs.Screens;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class ScreenService : IScreenService
{
    private readonly IScreenRepository _screenRepository;

    public ScreenService(IScreenRepository screenRepository)
    {
        _screenRepository = screenRepository;
    }

    // Create Screen
    public async Task<ScreenResponse> CreateScreenAsync(
        CreateScreenRequest request)
    {
        bool theatreExists =
            await _screenRepository.TheatreExistsAsync(request.TheatreId);

        if (!theatreExists)
        {
            throw new InvalidOperationException(
                $"Theatre with ID {request.TheatreId} does not exist.");
        }

        var screen = new Screen
        {
            TheatreId = request.TheatreId,
            ScreenName = request.ScreenName
        };

        var createdScreen =
            await _screenRepository.AddAsync(screen);

        return MapToResponse(createdScreen);
    }

    // Get All Screens
    public async Task<List<ScreenResponse>> GetAllScreensAsync(
        int? theatreId)
    {
        var screens =
            await _screenRepository.GetAllAsync(theatreId);

        return screens.Select(MapToResponse).ToList();
    }

    // Get Screen By ID
    public async Task<ScreenResponse?> GetScreenByIdAsync(int screenId)
    {
        var screen =
            await _screenRepository.GetByIdAsync(screenId);

        if (screen == null)
        {
            return null;
        }

        return MapToResponse(screen);
    }

    // Get Screens By Theatre ID
    public async Task<List<ScreenResponse>> GetScreensByTheatreIdAsync(
        int theatreId)
    {
        var screens =
            await _screenRepository.GetByTheatreIdAsync(theatreId);

        return screens.Select(MapToResponse).ToList();
    }

    // Update Screen
    public async Task<ScreenResponse?> UpdateScreenAsync(
        int screenId,
        UpdateScreenRequest request)
    {
        var screen =
            await _screenRepository.GetByIdAsync(screenId);

        if (screen == null)
        {
            return null;
        }

        bool theatreExists =
            await _screenRepository.TheatreExistsAsync(request.TheatreId);

        if (!theatreExists)
        {
            throw new InvalidOperationException(
                $"Theatre with ID {request.TheatreId} does not exist.");
        }

        screen.TheatreId = request.TheatreId;
        screen.ScreenName = request.ScreenName;

        await _screenRepository.UpdateAsync(screen);

        return MapToResponse(screen);
    }

    // Delete Screen
    public async Task<bool> DeleteScreenAsync(int screenId)
    {
        var screen =
            await _screenRepository.GetByIdAsync(screenId);

        if (screen == null)
        {
            return false;
        }

        bool hasShows =
            await _screenRepository.HasShowsAsync(screenId);

        if (hasShows)
        {
            throw new InvalidOperationException(
                "Cannot delete this screen because shows are associated with it.");
        }

        bool hasSeats =
            await _screenRepository.HasSeatsAsync(screenId);

        if (hasSeats)
        {
            throw new InvalidOperationException(
                "Cannot delete this screen because seats are associated with it.");
        }

        await _screenRepository.DeleteAsync(screen);

        return true;
    }

    // Map Entity to Response DTO
    private static ScreenResponse MapToResponse(Screen screen)
    {
        return new ScreenResponse
        {
            ScreenId = screen.ScreenId,
            TheatreId = screen.TheatreId,
            ScreenName = screen.ScreenName
        };
    }
}