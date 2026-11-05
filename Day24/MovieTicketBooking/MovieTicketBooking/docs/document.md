# Movie Ticket Booking System

## 1. Project Overview

The Movie Ticket Booking System is a backend Web API application developed using ASP.NET Core Web API and Entity Framework Core.

The application manages movies, theatres, screens, seats, shows, and customer bookings.

### Architecture

The project follows a layered architecture:

**Controller → Service → Repository → DbContext → SQL Server**

Each layer has a separate responsibility:

- **Controller:** Receives HTTP requests and sends responses.
- **Service:** Contains application logic and business rules.
- **Repository:** Handles database operations.
- **DbContext:** Connects the application with the database using Entity Framework Core.
- **SQL Server:** Stores application data.

### Technologies Used

- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Scalar / OpenAPI
- Postman
- Visual Studio
- Git and GitHub

---

# 2. Commit-wise Development

This section explains what was implemented in each Git commit and the concepts used.

## Commit 1: Initial Database Migration

**Commit Message:**

`feat: create initial movie booking database migration`

### What was implemented?

- Created the ASP.NET Core Web API project.
- Configured the application to use SQL Server.
- Installed the required Entity Framework Core packages.
- Created the initial database migration.
- Created the database using Entity Framework Core.
- Configured Scalar/OpenAPI for API documentation.

### Concepts Learned

**1. ASP.NET Core Web API**

Web API allows applications to communicate using HTTP requests. For example, a client can send a request to retrieve movie details.

**2. Entity Framework Core (EF Core)**

EF Core is an Object Relational Mapper (ORM). It allows us to work with database tables using C# classes instead of writing SQL queries for every operation.

**3. DbContext**

`ApplicationDbContext` communicates with the database. It contains DbSet properties representing database tables.

**4. Migration**

A migration records changes made to the database structure. It helps create or update database tables based on the model classes.

**5. Dependency Injection (DI)**

Dependency Injection provides the required objects to classes automatically instead of creating them manually.

---

## Commit 2: Add Movie Booking Entities

**Commit Message:**

`feat: add movie booking entities`

### What was implemented?

Created eight entity classes representing the database tables:

1. Movie
2. Theatre
3. Screen
4. Seat
5. Show
6. ShowSeat
7. Booking
8. BookingSeat

Configured `ApplicationDbContext` with DbSet properties and Fluent API configurations.

### Concepts Learned

**1. Entity Class**

An entity is a C# class that represents a database table. Its properties represent table columns.

For example, the Movie entity contains:

- MovieId
- Title
- Description
- DurationMinutes
- ReleaseDate
- Language
- Genre

**2. Primary Key**

A primary key uniquely identifies each record in a table.

Example: `MovieId` identifies each movie.

**3. Foreign Key**

A foreign key connects one table with another table.

Example: `TheatreId` in the Screen table connects a screen to its theatre.

**4. Navigation Property**

Navigation properties allow EF Core to access related entities.

Example: A Theatre can contain multiple Screens.

**5. One-to-Many Relationship**

One record in a parent table can be connected to multiple records in a child table.

Examples:

- One Theatre can have multiple Screens.
- One Screen can have multiple Seats.
- One Movie can have multiple Shows.

**6. Fluent API**

Fluent API is used inside `OnModelCreating()` to configure:

- Table relationships
- Required fields
- Column lengths
- Indexes
- Database constraints

**7. Enum**

An enum defines a fixed set of possible values.

Examples:

- `ShowSeatStatus`: Available, Booked
- `BookingStatus`: Confirmed, Cancelled

**8. Database Constraints**

Constraints protect the database from invalid data.

Examples:

- Movie duration must be greater than zero.
- Seat number must be greater than zero.
- Ticket price cannot be negative.
- A seat position must be unique within its screen and row.

---

## Commit 3: Implement Movie Management APIs

**Commit Message:**

`feat: implement movie management APIs`

### What was implemented?

Created the complete Movie module with:

- Create Movie
- Get All Movies
- Get Movie By ID
- Search Movies
- Update Movie
- Delete Movie

Created Movie DTOs, repository, service, and controller.

### Concepts Learned

**1. DTO (Data Transfer Object)**

DTOs define the data that enters or leaves an API.

Instead of directly exposing database entity classes, we use DTOs to control the data sent and received.

Examples:

- `CreateMovieRequest`
- `UpdateMovieRequest`
- `MovieResponse`

**2. Repository Pattern**

The repository pattern separates database operations from business logic.

For example, `MovieRepository` handles operations such as adding, updating, deleting, and retrieving movies.

**3. Service Layer**

The service layer contains business logic.

For example, before deleting a movie, the service checks whether the movie has any shows.

**4. CRUD Operations**

CRUD represents four basic database operations:

- Create – Add a new movie.
- Read – Get movie information.
- Update – Modify movie information.
- Delete – Remove a movie.

**5. Async and Await**

Asynchronous programming allows the application to perform database operations without blocking the current thread while waiting for the result.

Methods return `Task` or `Task<T>` and use `async` and `await`.

**6. Pagination**

Pagination divides a large list into smaller pages.

Example: Instead of returning 100 movies at once, the API can return 10 movies per page.

**7. Search and Filtering**

Search allows users to find movies using specific information, such as the movie title.

---

## Commit 4: Implement Theatre Management APIs

**Commit Message:**

`feat: implement theatre management APIs`

### What was implemented?

Created the Theatre module with:

- Create Theatre
- Get All Theatres
- Get Theatre By ID
- Update Theatre
- Delete Theatre

Added DTOs, repository, service, controller, and Dependency Injection registrations.

### Concepts Learned

**1. Theatre Entity**

The Theatre entity stores theatre information such as TheatreId, Name, and Location.

**2. Foreign Key Relationship**

A theatre can have multiple screens. The Screen table uses TheatreId to identify the theatre to which each screen belongs.

**3. Business Validation**

Before deleting a theatre, the application checks whether screens are associated with it.

This prevents deleting a theatre that still has dependent records.

**4. Dependency Injection Registration**

The repository and service are registered in `Program.cs` so that the application can provide them automatically to the required classes.

---

## Commit 5: Implement Screen Management APIs

**Commit Message:**

`feat: implement screen management APIs`

### What was implemented?

Created the Screen module with:

- Create Screen
- Get All Screens
- Get Screen By ID
- Get Screens By Theatre
- Update Screen
- Delete Screen

Added DTOs, repository, service, controller, and DI registrations.

### Concepts Learned

**1. Screen Entity**

A screen represents an individual movie screen inside a theatre.

Example: Screen 1, Screen 2, and Screen 3.

**2. Filtering By Foreign Key**

We can retrieve all screens belonging to a particular theatre using TheatreId.

**3. Parent-Child Relationship**

Theatre is the parent entity, and Screen is the child entity.

**4. Delete Validation**

A screen should not be deleted when seats or shows are associated with it.

The service checks these dependencies before allowing deletion.

---

## Commit 6: Implement Seat Management APIs

**Commit Message:**

`feat: implement seat management APIs`

### What was implemented?

Created the Seat module with:

- Create Seat
- Bulk Create Seats
- Get All Seats
- Get Seat By ID
- Get Seats By Screen
- Update Seat
- Delete Seat

Added DTOs, repository, service, controller, and DI registrations.

### Concepts Learned

**1. Seat Entity**

A seat represents a physical seat inside a particular screen.

It contains information such as:

- SeatId
- ScreenId
- RowName
- SeatNumber
- SeatType

**2. Bulk Insert**

Bulk insert allows multiple seat records to be added in a single API request instead of sending separate requests for every seat.

**3. Duplicate Validation**

The application checks duplicate seat positions within the same screen and row.

**4. Unique Composite Index**

A composite unique index uses multiple columns together to identify a unique record.

For seats, the combination of ScreenId, RowName, and SeatNumber must be unique.

**5. Foreign Key Validation**

Before creating seats, the application checks whether the requested screen exists.

---

## Commit 7: Implement Show Management APIs

**Commit Message:**

`feat: implement show management APIs with automatic show seat generation`

### What was implemented?

Created the Show module with:

- Create Show
- Get All Shows
- Get Show By ID
- Get Shows By Movie
- Get Shows By Theatre
- Update Show
- Delete Show
- Get Show Seats
- Get Available Seats
- Get Booked Seats

When a show is created, the application automatically creates ShowSeat records for all physical seats belonging to that screen.

Initially, every generated ShowSeat has the status Available.

### Concepts Learned

**1. Show Entity**

A Show represents a movie scheduled at a particular screen on a specific date and time.

It connects Movie and Screen using their foreign keys.

**2. ShowSeat Entity**

ShowSeat represents the availability of a physical seat for a particular show.

The same physical seat can be available for one show and booked for another show.

**3. Automatic Record Generation**

When a new show is created, the application retrieves all seats belonging to its screen and creates corresponding ShowSeat records.

**4. Enum Status**

ShowSeat uses two statuses:

- Available
- Booked

**5. Database Transaction**

A transaction groups multiple database operations into one unit.

For show creation:

1. Insert the Show record.
2. Generate ShowSeat records.
3. Save the changes.
4. Commit the transaction.

If an operation fails before the transaction commits, the changes can be rolled back.

**6. Schedule Overlap Validation**

The application checks whether another show is already scheduled in the same screen during an overlapping time period.

**7. Filtering**

Shows can be retrieved based on MovieId, Theatre, or date-related search criteria.

---

## Commit 8: Implement Booking Management APIs

**Commit Message:**

`feat: implement booking management APIs with seat reservation and cancellation`

### What was implemented?

Created the Booking module with:

- Create Booking
- Get All Bookings
- Get Booking By ID
- Get Bookings By Show
- Cancel Booking

Booking creation validates seat availability, calculates the total amount, creates booking records, and updates the selected ShowSeat statuses.

### Concepts Learned

**1. Booking Entity**

Booking stores customer and booking information such as:

- BookingId
- BookingReference
- ShowId
- CustomerName
- CustomerEmail
- CustomerPhone
- BookingDate
- TotalAmount
- Status

**2. BookingSeat Entity**

BookingSeat connects a booking with the seats selected by the customer.

It also stores the price of each booked seat.

**3. Seat Availability Validation**

Before booking, the application checks:

1. The selected show exists.
2. Seat IDs are not empty.
3. Seat IDs do not contain duplicates.
4. All selected seats belong to the requested show.
5. All selected seats are Available.

**4. Total Amount Calculation**

The server calculates the amount using the show ticket price and the number of selected seats.

Example:

- Ticket price = ₹150
- Number of seats = 2
- Total amount = ₹150 × 2 = ₹300

**5. Database Transaction During Booking**

Booking creation involves multiple database operations:

1. Create the Booking record.
2. Generate the BookingReference.
3. Create BookingSeat records.
4. Change the selected ShowSeat statuses to Booked.
5. Commit the transaction.

These operations are handled in one transaction to prevent partial booking data.

**6. Cancellation**

When a confirmed booking is cancelled:

1. Booking status changes to Cancelled.
2. Related ShowSeat statuses change back to Available.
3. Changes are saved in a transaction.

The booking history is preserved instead of deleting the booking record.

---

## Commit 9: Add Custom Exceptions

**Commit Message:**

`feat: add custom exceptions for not found and business rule errors`

### What was implemented?

Created an `Exceptions` folder and added:

- `NotFoundException`
- `BusinessRuleException`

### Concepts Learned

**1. Custom Exception**

A custom exception is an exception class created by the developer for a particular application requirement.

**2. NotFoundException**

Used when a requested resource does not exist.

Example: A movie with MovieId 100 does not exist.

**3. BusinessRuleException**

Used when an operation violates an application business rule.

Example: Trying to book a seat that is already booked.

**4. Why Custom Exceptions?**

Custom exceptions make error handling clearer. They also help distinguish missing resources from business rule violations.

---

## Commit 10: Register Global Exception Handling Middleware

**Commit Message:**

`feat: register global exception handling middleware`

### What was implemented?

- Created `Middleware/GlobalExceptionHandler.cs`.
- Registered the global exception handler in `Program.cs`.
- Registered ProblemDetails.
- Configured the application to use exception handling middleware.

### Concepts Learned

**1. Middleware**

Middleware is software that is executed as part of the HTTP request and response pipeline.

It can handle requests, responses, and exceptions.

**2. Global Exception Handling**

Instead of writing separate exception-handling code in every controller, a global exception handler catches unhandled exceptions centrally.

**3. IExceptionHandler**

`IExceptionHandler` is an ASP.NET Core interface used to implement centralized exception handling.

**4. ProblemDetails**

ProblemDetails provides a standard structure for returning API error responses.

**5. HTTP Status Codes**

The global handler maps exceptions to appropriate HTTP status codes:

- 400 – Bad Request
- 404 – Not Found
- 409 – Conflict
- 500 – Internal Server Error

For unexpected server errors, the response contains a general message instead of exposing internal exception details.

**6. Logging**

The handler uses `ILogger` to record exception information for debugging and monitoring.

---

## Commit 11: Replace Built-in Exceptions with Custom Exceptions

**Commit Message:**

`refactor: replace built-in exceptions with custom exceptions across services`

### What was implemented?

Updated the service classes to use the newly created custom exceptions.

Updated services:

- MovieService
- TheatreService
- ScreenService
- SeatService
- ShowService
- BookingService

### Concepts Learned

**1. Refactoring**

Refactoring means improving existing code without changing the main functionality of the application.

**2. Replacing Built-in Exceptions**

Instead of using general exceptions for application-specific situations, the services now use:

- NotFoundException for missing resources.
- BusinessRuleException for business rule violations.

**3. Consistent Error Handling**

All services follow the same exception approach. The global exception handler can then convert these exceptions into standard HTTP error responses.

---

## Commit 12: Update Booking Controller to Use Global Exception Handling

**Commit Message:**

`refactor: update booking controller to use global exception handling`

### What was implemented?

Updated `BookingsController` to remove its local try-catch handling around booking operations.

The controller now allows exceptions from the service layer to reach the global exception handler.

### Concepts Learned

**1. Separation of Responsibilities**

The controller should mainly handle HTTP requests and responses.

The service should handle business logic.

The global exception handler should handle application exceptions that are allowed to propagate.

**2. Why Remove Local Try-Catch?**

If every controller handles exceptions separately, the API may return different error formats.

Centralized exception handling provides a consistent approach.

**3. CreatedAtAction**

`CreatedAtAction` returns HTTP 201 Created after successfully creating a booking. It also provides a route to retrieve the newly created booking.

**4. Controller Responsibilities**

The BookingsController handles:

- POST for creating a booking.
- GET for retrieving bookings.
- GET by ID for retrieving one booking.
- GET by show for retrieving bookings for a show.
- PUT for cancelling a booking.

---

# 3. Project Folder Structure

The following structure represents the project organization up to the latest recorded commit.

```text
HCL-Training
│
└── Day22
    │
    └── MovieTicketBooking
        │
        └── MovieTicketBooking
            │
            ├── Controllers
            │   ├── MoviesController.cs
            │   ├── TheatresController.cs
            │   ├── ScreensController.cs
            │   ├── SeatsController.cs
            │   ├── ShowsController.cs
            │   └── BookingsController.cs
            │
            ├── Data
            │   └── ApplicationDbContext.cs
            │
            ├── DTOs
            │   ├── Movies
            │   │   ├── CreateMovieRequest.cs
            │   │   ├── UpdateMovieRequest.cs
            │   │   └── MovieResponse.cs
            │   │
            │   ├── Theatres
            │   │   ├── CreateTheatreRequest.cs
            │   │   ├── UpdateTheatreRequest.cs
            │   │   └── TheatreResponse.cs
            │   │
            │   ├── Screens
            │   │   ├── CreateScreenRequest.cs
            │   │   ├── UpdateScreenRequest.cs
            │   │   └── ScreenResponse.cs
            │   │
            │   ├── Seats
            │   │   ├── CreateSeatRequest.cs
            │   │   ├── UpdateSeatRequest.cs
            │   │   ├── BulkCreateSeatsRequest.cs
            │   │   └── SeatResponse.cs
            │   │
            │   ├── Shows
            │   │   ├── CreateShowRequest.cs
            │   │   ├── UpdateShowRequest.cs
            │   │   └── ShowResponse.cs
            │   │
            │   └── Bookings
            │       ├── CreateBookingRequest.cs
            │       └── BookingResponse.cs
            │
            ├── Exceptions
            │   ├── NotFoundException.cs
            │   └── BusinessRuleException.cs
            │
            ├── Middleware
            │   └── GlobalExceptionHandler.cs
            │
            ├── Migrations
            │   ├── InitialCreate.cs
            │   ├── InitialCreate.Designer.cs
            │   └── ApplicationDbContextModelSnapshot.cs
            │
            ├── Models
            │   ├── Movie.cs
            │   ├── Theatre.cs
            │   ├── Screen.cs
            │   ├── Seat.cs
            │   ├── Show.cs
            │   ├── ShowSeat.cs
            │   ├── Booking.cs
            │   ├── BookingSeat.cs
            │   ├── ShowSeatStatus.cs
            │   └── BookingStatus.cs
            │
            ├── Repositories
            │   ├── Interfaces
            │   │   ├── IMovieRepository.cs
            │   │   ├── ITheatreRepository.cs
            │   │   ├── IScreenRepository.cs
            │   │   ├── ISeatRepository.cs
            │   │   ├── IShowRepository.cs
            │   │   └── IBookingRepository.cs
            │   │
            │   ├── MovieRepository.cs
            │   ├── TheatreRepository.cs
            │   ├── ScreenRepository.cs
            │   ├── SeatRepository.cs
            │   ├── ShowRepository.cs
            │   └── BookingRepository.cs
            │
            ├── Services
            │   ├── Interfaces
            │   │   ├── IMovieService.cs
            │   │   ├── ITheatreService.cs
            │   │   ├── IScreenService.cs
            │   │   ├── ISeatService.cs
            │   │   ├── IShowService.cs
            │   │   └── IBookingService.cs
            │   │
            │   ├── MovieService.cs
            │   ├── TheatreService.cs
            │   ├── ScreenService.cs
            │   ├── SeatService.cs
            │   ├── ShowService.cs
            │   └── BookingService.cs
            │
            ├── Properties
            │
            ├── appsettings.json
            ├── Program.cs
            └── MovieTicketBooking.csproj
```

> Note: This is the intended logical structure based on the files created so far. Check Solution Explorer for any differences in actual file names.

---

# 4. Important Concepts – Quick Revision

| Concept | Simple Meaning |
|---|---|
| ASP.NET Core Web API | Builds HTTP APIs |
| Entity Framework Core | Works with databases using C# |
| DbContext | Connects the application to the database |
| Migration | Updates database structure |
| Entity | C# class representing a database table |
| DTO | Defines request and response data |
| Repository Pattern | Separates database operations |
| Service Layer | Contains business logic |
| Dependency Injection | Provides required objects automatically |
| Async/Await | Handles asynchronous operations |
| CRUD | Create, Read, Update, Delete |
| LINQ | Queries and filters data using C# |
| Foreign Key | Connects related database tables |
| Navigation Property | Accesses related entities |
| Fluent API | Configures entity relationships and constraints |
| Transaction | Groups database operations into one unit |
| Middleware | Handles requests and responses in the pipeline |
| Custom Exception | Application-specific error class |
| ProblemDetails | Standard API error response format |
| Logging | Records application errors and events |
| Refactoring | Improves existing code structure |

---

# 5. Current Development Status

### Completed Work Up to the Latest Recorded Commit

- Initial project and database setup.
- Eight entity classes and database relationships.
- Movie management APIs.
- Theatre management APIs.
- Screen management APIs.
- Seat management APIs.
- Show management APIs with automatic ShowSeat generation.
- Booking creation and cancellation.
- Custom exceptions.
- Global exception handler registration.
- Custom exception usage in service classes.
- BookingController updated to use centralized exception handling.

### Next Planned Work

- Review remaining controllers for consistent global exception handling.
- Test all API modules using Postman.
- Test invalid inputs and business rules.
- Test booking and cancellation end to end.
- Verify database records and relationships.
- Complete final integration testing.

*These are planned activities, not confirmed completed test results.*

---

# 6. Git Commit History

```text
refactor: update booking controller to use global exception handling

refactor: replace built-in exceptions with custom exceptions across services

feat: register global exception handling middleware

feat: add custom exceptions for not found and business rule errors

feat: implement booking management APIs with seat reservation and cancellation

feat: implement show management APIs with automatic show seat generation

feat: implement seat management APIs

feat: implement screen management APIs

feat: implement theatre management APIs

feat: implement movie management APIs

feat: add movie booking entities

feat: create initial movie booking database migration
```

---

## 7. Purpose of This Document

This document records the development progress of the Movie Ticket Booking System commit by commit.

It explains:

- What was implemented in each commit.
- Which files and modules were created.
- Which programming concepts were used.
- Why those concepts are important.
- How the project is organized.

It can be used for project revision, technical interviews, and explaining the development process.