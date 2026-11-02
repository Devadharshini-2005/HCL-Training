using MovieTicketBooking.DTOs.Seats;
using MovieTicketBooking.Exceptions;
using MovieTicketBooking.Models;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services.Interfaces;

namespace MovieTicketBooking.Services;

public class SeatService : ISeatService
{
    private readonly ISeatRepository _seatRepository;

    public SeatService(ISeatRepository seatRepository)
    {
        _seatRepository = seatRepository;
    }

    // Create one seat
    public async Task<SeatResponse> CreateSeatAsync(
        CreateSeatRequest request)
    {
        bool screenExists = await _seatRepository
            .ScreenExistsAsync(request.ScreenId);

        if (!screenExists)
        {
            throw new NotFoundException(
                "The specified screen does not exist.");
        }

        bool seatExists = await _seatRepository.SeatExistsAsync(
            request.ScreenId,
            request.RowName,
            request.SeatNumber);

        if (seatExists)
        {
            throw new BusinessRuleException(
                "A seat with this row and seat number already exists in this screen.");
        }

        var seat = new Seat
        {
            ScreenId = request.ScreenId,
            RowName = request.RowName,
            SeatNumber = request.SeatNumber,
            SeatType = request.SeatType
        };

        var createdSeat = await _seatRepository.AddAsync(seat);

        return MapToResponse(createdSeat);
    }

    // Create multiple seats
    public async Task<List<SeatResponse>> CreateSeatsBulkAsync(
        BulkCreateSeatsRequest request)
    {
        if (request.Seats == null || request.Seats.Count == 0)
        {
            throw new BusinessRuleException(
                "At least one seat is required.");
        }

        var duplicateSeats = request.Seats
            .GroupBy(s => new
            {
                s.ScreenId,
                s.RowName,
                s.SeatNumber
            })
            .Any(g => g.Count() > 1);

        if (duplicateSeats)
        {
            throw new BusinessRuleException(
                "Duplicate seat positions were found in the request.");
        }

        var seatsToCreate = new List<Seat>();

        foreach (var item in request.Seats)
        {
            bool screenExists = await _seatRepository
                .ScreenExistsAsync(item.ScreenId);

            if (!screenExists)
            {
                throw new NotFoundException(
                    $"Screen ID {item.ScreenId} does not exist.");
            }

            bool seatExists = await _seatRepository.SeatExistsAsync(
                item.ScreenId,
                item.RowName,
                item.SeatNumber);

            if (seatExists)
            {
                throw new BusinessRuleException(
                    $"Seat {item.RowName}{item.SeatNumber} already exists in Screen ID {item.ScreenId}.");
            }

            seatsToCreate.Add(new Seat
            {
                ScreenId = item.ScreenId,
                RowName = item.RowName,
                SeatNumber = item.SeatNumber,
                SeatType = item.SeatType
            });
        }

        var createdSeats = await _seatRepository
            .AddRangeAsync(seatsToCreate);

        return createdSeats.Select(MapToResponse).ToList();
    }

    // Get seats belonging to a screen
    public async Task<List<SeatResponse>> GetSeatsByScreenIdAsync(
        int screenId)
    {
        bool screenExists = await _seatRepository
            .ScreenExistsAsync(screenId);

        if (!screenExists)
        {
            throw new NotFoundException(
                "The specified screen does not exist.");
        }

        var seats = await _seatRepository
            .GetByScreenIdAsync(screenId);

        return seats.Select(MapToResponse).ToList();
    }

    // Get a seat by ID
    public async Task<SeatResponse?> GetSeatByIdAsync(int seatId)
    {
        var seat = await _seatRepository.GetByIdAsync(seatId);

        if (seat == null)
        {
            return null;
        }

        return MapToResponse(seat);
    }

    // Update a seat
    public async Task<SeatResponse?> UpdateSeatAsync(
        int seatId,
        UpdateSeatRequest request)
    {
        var seat = await _seatRepository.GetByIdAsync(seatId);

        if (seat == null)
        {
            return null;
        }

        bool screenExists = await _seatRepository
            .ScreenExistsAsync(request.ScreenId);

        if (!screenExists)
        {
            throw new NotFoundException(
                "The specified screen does not exist.");
        }

        bool seatExists = await _seatRepository.SeatExistsAsync(
            request.ScreenId,
            request.RowName,
            request.SeatNumber,
            seatId);

        if (seatExists)
        {
            throw new BusinessRuleException(
                "A seat with this row and seat number already exists in this screen.");
        }

        seat.ScreenId = request.ScreenId;
        seat.RowName = request.RowName;
        seat.SeatNumber = request.SeatNumber;
        seat.SeatType = request.SeatType;

        await _seatRepository.UpdateAsync(seat);

        return MapToResponse(seat);
    }

    // Delete a seat
    public async Task<bool> DeleteSeatAsync(int seatId)
    {
        var seat = await _seatRepository.GetByIdAsync(seatId);

        if (seat == null)
        {
            return false;
        }

        bool hasShowSeats = await _seatRepository
            .HasShowSeatsAsync(seatId);

        bool hasBookingSeats = await _seatRepository
            .HasBookingSeatsAsync(seatId);

        if (hasShowSeats || hasBookingSeats)
        {
            throw new BusinessRuleException(
    "Cannot delete this seat because it is associated with show or booking history.");

        }

        await _seatRepository.DeleteAsync(seat);

        return true;
    }

    // Map Seat entity to SeatResponse DTO
    private static SeatResponse MapToResponse(Seat seat)
    {
        return new SeatResponse
        {
            SeatId = seat.SeatId,
            ScreenId = seat.ScreenId,
            RowName = seat.RowName,
            SeatNumber = seat.SeatNumber,
            SeatType = seat.SeatType
        };
    }
}