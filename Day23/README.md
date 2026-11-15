# Day 23 – Movie Ticket Booking: Resource-Based Authorization

## Overview
Today, I continued working on the Movie Ticket Booking System and implemented Resource-Based Authorization to secure booking operations.

The main objective was to ensure that users can access or manage only the bookings they are authorized to access.

## Technologies Used
- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication

## Topics Covered
- Resource-Based Authorization
- Authorization Requirements
- Authorization Handlers
- Custom Authorization Policies
- Booking Ownership Validation
- Dependency Injection
- Secure API Endpoints

## Implementation
- Implemented custom authorization requirements and handlers.
- Created booking ownership validation logic.
- Configured the Resource-Based Authorization policy.
- Checked whether the logged-in customer owns the requested booking.
- Restricted booking access based on user ownership.
- Integrated authorization logic into the booking service.

## Learning Outcomes
- Understood Resource-Based Authorization in ASP.NET Core.
- Learned how authorization requirements and handlers work.
- Implemented ownership-based access control.
- Understood how to protect individual resources.
- Improved the security of booking operations.

## GitHub Commits
- `MovieTicketBooking backend files`
- `Front files for MovieTicketBooking`
- `feat: implement resource-based booking authorization`

## Status
Completed