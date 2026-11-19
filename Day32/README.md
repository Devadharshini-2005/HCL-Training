# FoodHub – Food Menu Management System

FoodHub is a web-based Food Menu Management System developed using **ASP.NET Core Web API, Entity Framework Core, SQL Server, and Angular**. It allows administrators to manage food items and categories, while customers can view the food menu. The application includes JWT authentication and role-based authorization to secure its APIs and management operations.

## Features

### Food Item Management
- View all food items.
- View food item details.
- Add new food items.
- Update existing food items.
- Delete food items.
- Search food items.

### Category Management
- View all food categories.
- View category details.
- Add new categories.
- Update categories.
- Delete categories.
- Associate food items with categories.

### Authentication and Authorization
- User registration and login.
- Password hashing and verification.
- JWT token generation and validation.
- Role-based authorization using Admin and Customer roles.
- Protected API endpoints.
- Angular authentication service.
- JWT token storage and handling.
- HTTP interceptor for attaching tokens to API requests.
- Role-based navigation and UI visibility.
- Login and logout functionality.

### Role Permissions

| Functionality | Admin | Customer |
|---|---|---|
| View food items | Yes | Yes |
| View food item details | Yes | Yes |
| Search food items | Yes | Yes |
| Add food items | Yes | No |
| Edit food items | Yes | No |
| Delete food items | Yes | No |
| View categories | Yes | Yes |
| View category details | Yes | Yes |
| Add categories | Yes | No |
| Edit categories | Yes | No |
| Delete categories | Yes | No |

## Technology Stack

### Backend
- C#
- ASP.NET Core Web API
- .NET 10
- Entity Framework Core 10
- Repository Pattern
- Service Layer
- Dependency Injection
- JWT Authentication
- Role-Based Authorization

### Database
- Microsoft SQL Server Express
- Entity Framework Core Migrations

### Frontend
- Angular
- TypeScript
- HTML
- CSS
- Angular Router
- Angular Forms
- HttpClient
- HTTP Interceptors

### Tools
- Visual Studio 2022
- Visual Studio Code
- SQL Server Management Studio (SSMS)
- Postman
- Git
- GitHub

## Project Structure

```text
FoodHub/
├── Backend/
│   └── FoodHub/
│       ├── Controllers/
│       ├── Data/
│       ├── DTOs/
│       ├── Models/
│       ├── Repositories/
│       │   ├── Interfaces/
│       │   └── Implementations/
│       ├── Services/
│       │   ├── Interfaces/
│       │   └── Implementations/
│       ├── Constants/
│       ├── appsettings.json
│       └── Program.cs
│
├── Frontend/
│   └── FoodHub/
│       └── src/
│           └── app/
│               ├── components/
│               ├── models/
│               ├── services/
│               ├── interceptors/
│               ├── app.routes.ts
│               ├── app.ts
│               └── app.html
│
└── docs/
    ├── Phase3/
    └── Phase4/
```

## Architecture

FoodHub follows a layered architecture to separate responsibilities.

```text
Angular Frontend
       |
       | HTTP Requests
       v
ASP.NET Core Web API
       |
       ├── Controllers
       |
       ├── Services
       |
       ├── Repositories
       |
       v
Entity Framework Core
       |
       v
SQL Server Database
```

- **Controllers:** Handle HTTP requests and responses.
- **Services:** Contain application business logic.
- **Repositories:** Handle database operations.
- **Entity Framework Core:** Provides ORM functionality for database access.
- **DTOs:** Transfer data between the client and API.
- **Angular Services:** Handle API communication and authentication.
- **HTTP Interceptor:** Automatically attaches JWT tokens to outgoing requests.

## Authentication Flow

1. User registers using the registration form.
2. Password is hashed before being stored in the database.
3. User logs in with email and password.
4. Backend verifies the credentials.
5. JWT token is generated and returned.
6. Angular stores the token in local storage.
7. HTTP interceptor attaches the token to API requests.
8. Backend validates the token.
9. Role-based policies determine whether the user is authorized to perform the requested operation.

## User Roles

### Admin
- Manage food items.
- Manage categories.
- Access administrative operations.
- View food items and categories.

### Customer
- Browse food items.
- View food item details.
- Search food items.
- Browse categories.
- Access only permitted read operations.

## API Endpoints

### Authentication

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/auth/register` | Register a user |
| POST | `/api/auth/login` | Login and receive JWT |

### Food Items

| Method | Endpoint | Access |
|---|---|---|
| GET | `/api/fooditems` | Admin, Customer |
| GET | `/api/fooditems/{id}` | Admin, Customer |
| GET | `/api/fooditems/search?searchTerm={term}` | Admin, Customer |
| POST | `/api/fooditems` | Admin |
| PUT | `/api/fooditems/{id}` | Admin |
| DELETE | `/api/fooditems/{id}` | Admin |

### Categories

| Method | Endpoint | Access |
|---|---|---|
| GET | `/api/categories` | Admin, Customer |
| GET | `/api/categories/{id}` | Admin, Customer |
| POST | `/api/categories` | Admin |
| PUT | `/api/categories/{id}` | Admin |
| DELETE | `/api/categories/{id}` | Admin |

## Database

Database: `FoodHubDB`

Database Server: SQL Server Express (`.\SQLEXPRESS`)

Main entities:
- FoodItem
- Category
- User

Relationships:
- A category can contain multiple food items.
- Each food item belongs to a category.
- Users have assigned roles for authorization.

## Running the Project

### Prerequisites

- .NET 10 SDK
- SQL Server Express
- Node.js and npm
- Angular CLI
- Visual Studio 2022
- Visual Studio Code

### Backend Setup

1. Open the backend solution in Visual Studio.
2. Configure the database connection string in `appsettings.json`.
3. Ensure SQL Server Express is running.
4. Apply EF Core migrations if required.
5. Run the backend application.

Backend URLs:

```text
HTTPS: https://localhost:7180
HTTP:  http://localhost:5177
```

### Frontend Setup

Navigate to the Angular project:

```bash
cd Frontend/FoodHub
```

Install dependencies:

```bash
npm install
```

Start Angular:

```bash
ng serve
```

Frontend URL:

```text
http://localhost:4200
```

## Manual API Verification

Use Postman to manually verify the API endpoints.

- Verify registration with valid and invalid data.
- Verify login with correct and incorrect credentials.
- Verify food item CRUD operations.
- Verify category CRUD operations.
- Verify Admin permissions.
- Verify Customer restrictions.
- Verify unauthorized requests.
- Verify JWT token validation.

## Project Progress

| Phase | Description | Status |
|---|---|---|
| Phase 1 | Food Item Management | Completed |
| Phase 2 | Food Item Enhancements | Completed |
| Phase 3 | Category Management | Completed |
| Phase 4 | Authentication and Authorization | Completed |

### Remaining planned work

- Customer menu filtering and sorting.
- Additional planned application-wide architecture refinements.
- Final integration and refinement.

## Security

- Passwords are stored as hashes, not plain text.
- JWT authentication is used for secured API access.
- Role-based authorization restricts Admin-only operations.
- Customer registration does not allow users to assign themselves Admin roles.
- API authorization is enforced on the backend; hiding UI controls is not a substitute for API security.

## Developer

Developed as part of full-stack application development practice using .NET and Angular.

---

**FoodHub – Food Menu Management System**
