# Day 24 – Middleware and Diagnostics

## Overview
Today, I continued working on the Movie Ticket Booking System and focused on understanding and implementing custom middleware in ASP.NET Core.

I also implemented authentication and authorization diagnostic middleware to understand the request pipeline and debug authorization-related issues.

## Technologies Used
- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication

## Topics Covered
- Custom Middleware
- HTTP Request and Response Pipeline
- Authentication Middleware
- Authorization Middleware
- Middleware Execution Order
- Endpoint Metadata
- Authentication Diagnostics
- Authorization Diagnostics
- Controller Configuration

## Implementation
- Created custom authentication diagnostic middleware.
- Created authorization diagnostic middleware.
- Implemented endpoint metadata inspection.
- Checked authentication and authorization information during HTTP requests.
- Used diagnostic logs and breakpoints to understand middleware execution.
- Updated the Booking Controller.
- Configured the middleware pipeline in the application.
- Tested middleware behavior with API endpoints.

## Learning Outcomes
- Understood the ASP.NET Core middleware pipeline.
- Learned how custom middleware processes HTTP requests and responses.
- Understood the importance of middleware execution order.
- Learned how authentication and authorization work within the request pipeline.
- Used diagnostic middleware for debugging and troubleshooting.
- Improved understanding of endpoint metadata and controller configuration.

## GitHub Commits
- `feat(day24): add movie ticket booking project`
- `feat(middleware): add authentication and authorization diagnostics`
- `feat: implement resource-based booking authorization`
- `Update booking controller and middleware configuration`

## Status
Completed