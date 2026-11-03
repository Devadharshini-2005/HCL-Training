# CinemaBook — Angular + TypeScript frontend

This frontend is built from the uploaded MovieTicketBooking backend controllers and DTOs. It uses Angular standalone components, HttpClient, RxJS, JWT bearer interception, and role-aware navigation.

## Run
1. Start your ASP.NET Core backend on `https://localhost:7057`.
2. Open a terminal in this folder.
3. Run `npm install` then `npm start`.
4. Browse `http://localhost:4200`.

API base URL: `src/environments/environment.ts` (`https://localhost:7057/api`).

## Backend CORS requirement
Your supplied `Program.cs` did not include CORS. Add this before `builder.Build()`:

```csharp
builder.Services.AddCors(options => options.AddPolicy("AngularPolicy", policy =>
    policy.WithOrigins("http://localhost:4200")
          .AllowAnyHeader()
          .AllowAnyMethod()));
```

Add `app.UseCors("AngularPolicy");` after `app.UseHttpsRedirection()` and before `app.UseAuthentication()`. Keep authentication before authorization. Do not register duplicate CORS policies.

## Roles / permissions
- Register creates a Customer account (the backend assigns the role). The seeded Admin account is created by your backend seeder; login with its configured credentials.
- Admin routes and admin API operations require the `Admin` role.
- Customer seat booking and cancellation require the `Customer` role.
- Public browsing is used for GET movies, theatres, screens, shows and show seats where your controllers allow anonymous access.
- JWT token is stored in localStorage and attached as `Authorization: Bearer <token>`.

## Implemented endpoints
- Auth: POST `/Auth/register`, POST `/Auth/login`.
- Movies: GET `/movies` (pageNumber, pageSize, title), GET `/movies/{id}`, GET `/movies/search?title=...`, POST `/movies`, PUT `/movies/{id}`, DELETE `/movies/{id}`.
- Theatres: GET `/Theatres`, GET `/Theatres/{id}`, POST/PUT/DELETE `/Theatres`.
- Screens: GET `/Screens`, GET `/Screens/{id}`, GET `/Screens/theatre/{theatreId}`, POST/PUT/DELETE `/Screens`.
- Seats: GET `/Seats/{id}`, GET `/Seats/screen/{screenId}`, POST `/Seats`, POST `/Seats/bulk`, PUT/DELETE `/Seats/{id}`. Bulk body: `{ "seats": [ { "screenId": 1, "rowName": "A", "seatNumber": 1, "seatType": "Regular" } ] }`.
- Shows: GET `/shows` (date, movieId, theatreId), GET `/shows/{id}`, GET `/shows/movie/{movieId}`, GET `/shows/theatre/{theatreId}`, GET `/shows/search?date=...`, GET `/shows/{showId}/seats`, `/available`, `/booked`, POST/PUT/DELETE `/shows`.
- Bookings: POST `/Bookings`, GET `/Bookings` (Admin only; showId/status filters), GET `/Bookings/{id}`, GET `/Bookings/show/{showId}` (Admin), PUT `/Bookings/{id}/cancel` (Customer).

## Exact request DTOs used
- Register: `fullName`, `email`, `password`, optional `phoneNumber`.
- Login: `email`, `password`.
- Movie: `title`, `description`, `durationMinutes`, `releaseDate` (`YYYY-MM-DD`), `language`, `genre`.
- Theatre: `name`, `location`. Screen: `theatreId`, `screenName`.
- Seat: `screenId`, `rowName`, `seatNumber`, `seatType`.
- Show: `movieId`, `screenId`, `startTime`, `endTime`, `ticketPrice`.
- Booking: `showId`, `customerName`, `customerEmail`, `customerPhone`, `seatIds` (array of seat IDs).

## Admin data management
The Admin dashboard includes create/update/delete and list operations for Movies, Theatres, Screens, Seats and Shows, plus booking list filters. The JSON editor sends request bodies unchanged using the DTO field names above. This avoids inventing fields that are not in the provided DTOs. For bulk seat creation use the Seats JSON body with the `seats` array described above.

## Notes
- The backend document does not include `AddCors`; CORS must be configured in the backend as shown above.
- `GET /Bookings` is Admin-only in the supplied controller. The supplied controller excerpt does not expose a customer booking-history endpoint; the My Bookings screen calls this endpoint and will receive 403 for Customer. This is a backend contract limitation, not an Angular issue. To show customer history, the backend needs a customer-scoped GET endpoint (not added here because you asked to preserve backend features).
- `GET /Bookings/{id}` is allowed for Customer and Admin.
- Admin create/update/delete actions use the exact request DTO JSON.
