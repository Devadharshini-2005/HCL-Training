
# Library Management System

A C# console application for managing books, library members, and borrowing records.

## Technologies

- C#
- .NET 10
- Entity Framework Core
- SQL Server
- Fluent API
- Repository Pattern
- Service Layer

## Features

### Book Management
- Add books
- View all books
- Search by book ID
- Update book details
- Delete books when allowed

### Member Management
- Register members
- View all members
- Search by member ID
- Update member details
- Delete members when allowed

### Borrowing Management
- Borrow available books
- View borrowing history
- Search borrowing records
- Return borrowed books
- Update available-copy counts

## Database

Database name: `LibraryManagementDB`

SQL Server instance: `.\SQLEXPRESS`

Tables:
- Books
- Members
- Borrowings

## Setup

1. Install the .NET 10 SDK.
2. Install SQL Server Express.
3. Restore NuGet packages:

   ```bash
   dotnet restore
   ```

4. Build the project:

   ```bash
   dotnet build
   ```

5. Apply database migrations:

   ```bash
   dotnet ef database update
   ```

6. Run the application after the main menu is connected in the final development stage:

   ```bash
   dotnet run
   ```

## Architecture

The project separates models, database configuration, repositories, services, and console menus.

Dependency Injection and main-menu registration are added in the final development stage.
