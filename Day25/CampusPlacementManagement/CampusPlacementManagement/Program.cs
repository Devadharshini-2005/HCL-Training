using CampusPlacementManagement.Authorization.Handlers;
using CampusPlacementManagement.Authorization.Requirements;
using CampusPlacementManagement.Data;
using CampusPlacementManagement.Data.Seed;
using CampusPlacementManagement.Middleware;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Repositories.Implementations;
using CampusPlacementManagement.Repositories.Interfaces;
using CampusPlacementManagement.Services.Implementations;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Register controllers
builder.Services.AddControllers();

// Register Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register SQL Server and ApplicationDbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Register Data Protection services
builder.Services.AddDataProtection();

// Configure ASP.NET Core Identity
builder.Services
    .AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");

var secretKey = jwtSettings["Key"]
    ?? throw new InvalidOperationException("JWT Key is missing.");

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
        options.TokenValidationParameters = new TokenValidationParameters
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

// Configure Authorization

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("CollegeAccess", policy =>
        {
            policy.Requirements.Add(
                new CollegeAccessRequirement());
        });

        options.AddPolicy("CompanyAccess", policy =>
        {
            policy.Requirements.Add(
                new CompanyAccessRequirement());
        });
    });

    // Register JWT Token Service
    builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();

// College Repository
builder.Services.AddScoped<ICollegeRepository, CollegeRepository>();

// College Service
builder.Services.AddScoped<ICollegeService, CollegeService>();

// Placement Officer Repository
builder.Services.AddScoped<
    IPlacementOfficerRepository,
    PlacementOfficerRepository>();

// Placement Officer Service
builder.Services.AddScoped<
    IPlacementOfficerService,
    PlacementOfficerService>();

// Company Management
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyService, CompanyService>();

builder.Services.AddScoped<ICompanyHRRepository, CompanyHRRepository>();
builder.Services.AddScoped<ICompanyHRService, CompanyHRService>();

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IStudentService, StudentService>();


builder.Services.AddScoped<IJobRepository, JobRepository>();

builder.Services.AddScoped<IJobService, JobService>();

builder.Services.AddScoped<
    IJobApplicationRepository,
    JobApplicationRepository>();

builder.Services.AddScoped<
    IJobApplicationService,
    JobApplicationService>();

// Recruitment Drive
builder.Services.AddScoped<
    IRecruitmentDriveRepository,
    RecruitmentDriveRepository>();

builder.Services.AddScoped<
    IRecruitmentDriveService,
    RecruitmentDriveService>();

// Interview Round
builder.Services.AddScoped<
    IInterviewRoundRepository,
    InterviewRoundRepository>();

builder.Services.AddScoped<
    IInterviewRoundService,
    InterviewRoundService>();

// Interview Schedule
builder.Services.AddScoped<
    IInterviewScheduleRepository,
    InterviewScheduleRepository>();

builder.Services.AddScoped<
    IInterviewScheduleService,
    InterviewScheduleService>();

builder.Services.AddScoped<IAuthorizationHandler, CollegeAccessHandler>();

builder.Services.AddScoped<IAuthorizationHandler, CompanyAccessHandler>();

var app = builder.Build();

// Global Exception Middleware
app.UseMiddleware<GlobalExceptionMiddleware>();

// Seed Identity roles
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider
        .GetRequiredService<RoleManager<IdentityRole>>();

    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<ApplicationUser>>();

    await IdentitySeeder.SeedRolesAsync(roleManager);

    await IdentitySeeder.SeedSuperAdminAsync(
        userManager,
        app.Configuration);
}

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();