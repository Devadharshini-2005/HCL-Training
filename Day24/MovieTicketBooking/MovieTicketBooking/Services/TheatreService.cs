using MovieTicketBooking.DTOs.Theatres;
using MovieTicketBooking.Exceptions;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class TheatreService : ITheatreService
{
    private readonly ITheatreRepository _theatreRepository;

    public TheatreService(ITheatreRepository theatreRepository)
    {
        _theatreRepository = theatreRepository;
    }

    // Create Theatre
    public async Task<TheatreResponse> CreateTheatreAsync(
        CreateTheatreRequest request)
    {
        var theatre = new Theatre
        {
            Name = request.Name,
            Location = request.Location
        };

        var createdTheatre =
            await _theatreRepository.AddAsync(theatre);

        return MapToResponse(createdTheatre);
    }

    // Get All Theatres
    public async Task<List<TheatreResponse>> GetAllTheatresAsync()
    {
        var theatres = await _theatreRepository.GetAllAsync();

        return theatres.Select(MapToResponse).ToList();
    }

    // Get Theatre By ID
    public async Task<TheatreResponse?> GetTheatreByIdAsync(int theatreId)
    {
        var theatre =
            await _theatreRepository.GetByIdAsync(theatreId);

        if (theatre == null)
        {
            return null;
        }

        return MapToResponse(theatre);
    }

    // Update Theatre
    public async Task<TheatreResponse?> UpdateTheatreAsync(
        int theatreId,
        UpdateTheatreRequest request)
    {
        var theatre =
            await _theatreRepository.GetByIdAsync(theatreId);

        if (theatre == null)
        {
            return null;
        }

        theatre.Name = request.Name;
        theatre.Location = request.Location;

        await _theatreRepository.UpdateAsync(theatre);

        return MapToResponse(theatre);
    }

    // Delete Theatre
    public async Task<bool> DeleteTheatreAsync(int theatreId)
    {
        var theatre =
            await _theatreRepository.GetByIdAsync(theatreId);

        if (theatre == null)
        {
            return false;
        }

        bool hasScreens =
            await _theatreRepository.HasScreensAsync(theatreId);

        if (hasScreens)
        {
            throw new BusinessRuleException(
    "Cannot delete this theatre because screens are associated with it.");

        }

        await _theatreRepository.DeleteAsync(theatre);

        return true;
    }

    // Map Entity to Response DTO
    private static TheatreResponse MapToResponse(Theatre theatre)
    {
        return new TheatreResponse
        {
            TheatreId = theatre.TheatreId,
            Name = theatre.Name,
            Location = theatre.Location
        };
    }
}