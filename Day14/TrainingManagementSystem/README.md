# TrainingManagementSystem

Training Course Enrollment and Fee Management System

## Stack
- Frontend: HTML, CSS, JavaScript
- Backend: ASP.NET Core Web API (.NET 10)
- Database: SQL Server via Entity Framework Core
- Initial demo authentication: role-based Admin / Student login

> This is a training/demo application. Login is intentionally simple for classroom use and is NOT production authentication. Before real deployment, use ASP.NET Core Identity or secure password hashing, JWT/cookies, HTTPS, authorization policies, and secret management.

## 1. Requirements
- .NET 10 SDK
- SQL Server / SQL Server Express / LocalDB
- Visual Studio 2022 or newer with ASP.NET and web development workload
- VS Code or any browser for the frontend

## 2. Configure database
Open `backend/TrainingManagementSystem.Api/appsettings.json` and update `DefaultConnection`.

Default LocalDB connection:
`Server=(localdb)\\MSSQLLocalDB;Database=TrainingManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true`

If using SQL Express, for example:
`Server=.\\SQLEXPRESS;Database=TrainingManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true`

## 3. Run backend
Open a terminal in `backend/TrainingManagementSystem.Api` and run:

```bash
dotnet restore
dotnet run
```

The app creates the database and seeds five courses plus demo users at startup.
The API prints the local URLs in the terminal. The frontend defaults to `http://localhost:5080`.
If your backend uses a different URL/port, edit `frontend/js/config.js`.

Demo accounts:
- Admin: `admin@training.local` / `Admin@123`
- Student: `student@training.local` / `Student@123`

These demo credentials are seeded for learning only.

## 4. Run frontend
Open `frontend/index.html` with VS Code Live Server, or serve the `frontend` folder on `http://localhost:5500`.
Do not open the HTML as `file://` if your browser blocks API requests. Use Live Server.

## 5. Features
- Admin and student login views
- Admin: view course catalogue, create courses, view learners, enrollments, payment attempts, reports
- Student: view courses, register a student account, enroll in a course, view own enrollments, simulate payment
- Case-insensitive course search and fee sorting
- Course seat checks and active duplicate-enrollment prevention
- Cancellation releases a seat
- Simulated payment history
- Database persistence through EF Core / SQL Server

## Notes
- Data persists in SQL Server, unlike the original in-memory console exercise.
- Payment is simulated; no real money is processed.
- This version prioritizes a runnable website and clear separation between frontend and backend. It does not implement every optional C# classroom exercise (such as `ref`, `out`, `in`, generic demo methods, Queue, Stack, and LINQ comparison), because those are language exercises rather than web features.
