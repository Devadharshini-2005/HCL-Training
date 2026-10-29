using LibraryManagement.Data;
using LibraryManagement.Menus;
using LibraryManagement.Repositories;
using LibraryManagement.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// Register database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        @"Server=.\SQLEXPRESS;Database=LibraryManagementDB;Trusted_Connection=True;TrustServerCertificate=True;",
        sqlOptions => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null)));
// Register repositories
builder.Services.AddScoped<BookRepository>();
builder.Services.AddScoped<MemberRepository>();
builder.Services.AddScoped<BorrowingRepository>();

// Register services
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<BorrowingService>();

// Register menus
builder.Services.AddScoped<BookMenu>();
builder.Services.AddScoped<MemberMenu>();
builder.Services.AddScoped<BorrowingMenu>();
builder.Services.AddScoped<MainMenu>();

// Build the application
using var host = builder.Build();

// Create a dependency injection scope
using var scope = host.Services.CreateScope();

// Resolve and display the main menu
var mainMenu = scope.ServiceProvider.GetRequiredService<MainMenu>();

mainMenu.ShowMenu();