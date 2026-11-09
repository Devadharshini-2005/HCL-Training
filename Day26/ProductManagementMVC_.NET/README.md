# Product Management MVC

A sample ASP.NET Core MVC application using .NET 10, EF Core 10 and SQL Server.

## Features

- Category CRUD
- Product CRUD
- Product search by name
- Category filtering
- Combined search and filtering
- Sorting by product name, price and category
- Database-level pagination (10, 20 or 50 records)
- Product and category validation
- Duplicate category-name prevention
- Prevent deletion of categories that contain products
- Dashboard statistics

## Architecture

The project uses a simple layered structure:

- Controllers
- Models
- ViewModels
- Services / Interfaces
- Repositories / Interfaces
- Data
- Views

## Requirements

- .NET 10 SDK
- SQL Server Express or SQL Server
- Visual Studio 2026 or compatible IDE

## Setup

1. Extract the ZIP.
2. Open `ProductManagementMVC.csproj` in Visual Studio.
3. Update `DefaultConnection` in `appsettings.json` to match your SQL Server instance.
4. Open Package Manager Console and run:

   ```powershell
   Add-Migration InitialCreate
   Update-Database
   ```

   Alternatively, from the project directory:

   ```bash
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   ```

5. Run the application.

The default route opens the Dashboard.

## Important notes

- The project source includes no generated migration files. Create the initial migration for your own database using the commands above.
- Product search, category filtering, sorting and pagination are composed as `IQueryable` operations and executed by SQL Server through EF Core.
- The inventory value is the sum of the current product prices. It is not a stock-quantity valuation because this learning project does not track stock quantities.
- Bootstrap is used minimally through its CDN. An internet connection is required to load the Bootstrap stylesheet and validation scripts.
- This is a learning project and does not include authentication or authorization.
