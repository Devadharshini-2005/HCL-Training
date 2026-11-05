using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Authorization;

public class BookingOwnerHandler :
    AuthorizationHandler<BookingOwnerRequirement, Booking>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        BookingOwnerRequirement requirement,
        Booking booking)
    {
        // Get logged-in user's ID from JWT
        var userId = context.User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        // Check booking ownership
        if (int.TryParse(userId, out int currentUserId) &&
            currentUserId == booking.UserId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}