
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieTicketBooking.Authorization;
using MovieTicketBooking.Data;
using MovieTicketBooking.Middleware;
using MovieTicketBooking.Repositories;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services;
using MovieTicketBooking.Services.Interfaces;
using Scalar.AspNetCore;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Exception Handling
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Controllers and OpenAPI
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Movie Dependencies
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();

// Theatre Dependencies
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<ITheatreService, TheatreService>();

// Screen Dependencies
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<IScreenService, ScreenService>();

// Seat Dependencies
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ISeatService, SeatService>();

// Show Dependencies
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IShowService, ShowService>();

// Booking Dependencies
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();

// Authentication Dependencies
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<JwtService>();

builder.Services.AddHttpContextAccessor();

// JWT Authentication Configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");

var secretKey = jwtSettings["SecretKey"]!;

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey)),

                ClockSkew = TimeSpan.Zero
            };
    });

// Resource-Based Authorization
builder.Services.AddScoped<
    IAuthorizationHandler,
    BookingOwnerHandler>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("BookingOwner", policy =>
    {
        policy.Requirements.Add(
            new BookingOwnerRequirement());
    });
});

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

// Exception Handler
app.UseExceptionHandler();

// OpenAPI and Scalar
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// HTTPS Redirection
app.UseHttpsRedirection();

// Routing
app.UseRouting();

// CORS
app.UseCors("AngularPolicy");

// Endpoint Metadata Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<EndpointMetadataMiddleware>();
}

// Authentication
app.UseAuthentication();

// Authentication and Authorization Diagnostics
if (app.Environment.IsDevelopment())
{
    app.UseMiddleware<AuthenticationDiagnosticsMiddleware>();
    app.UseMiddleware<AuthorizationDiagnosticsMiddleware>();
}

// Authorization
app.UseAuthorization();

// Map Controllers
app.MapControllers();

app.Run();
