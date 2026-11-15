# Day 25 – Campus Placement Management System

## Overview
Today, I developed the Campus Placement Management System using ASP.NET Core Web API. The project manages colleges, placement officers, companies, HR representatives, students, job opportunities, applications, recruitment drives, and interviews.

I also implemented authentication, resource-based authorization, validation, exception handling, reports, and unit testing.

## Technologies Used
- C#
- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- JWT Authentication
- Swagger / OpenAPI
- xUnit
- Angular (Frontend Integration)

## Modules Implemented
- College Management
- Placement Officer Management
- Company Management
- Company HR Management
- Student Management
- Job Management
- Job Application Management
- Recruitment Drive Management
- Interview Management
- Placement and Recruitment Reports

## Topics Covered

### 1. Backend Development
- Created database models and entity relationships.
- Configured Entity Framework Core and database mappings.
- Implemented CRUD operations and business workflows.
- Used Dependency Injection to register services.

### 2. Authentication and Authorization
- Implemented JWT authentication.
- Added refresh token rotation.
- Implemented resource-based authorization for colleges and companies.
- Restricted access to students, jobs, and job applications based on resource ownership.

### 3. Business Workflows
- Implemented college and placement officer registration and approval.
- Implemented company and HR registration and approval.
- Implemented student enrollment and approval.
- Developed job management and eligibility targeting.
- Implemented job application workflow and status history.
- Developed recruitment drives and interview management.

### 4. Exception Handling and Validation
- Implemented global exception handling middleware.
- Added validation attributes to request DTOs.
- Configured CORS for Angular frontend integration.

### 5. Reporting and Testing
- Implemented placement and recruitment reports.
- Created unit tests for college and placement officer services.
- Added unit tests for company, company HR, and student services.
- Added unit tests for job, job application, and recruitment drive services.

## Learning Outcomes
- Gained practical experience in developing a complete backend application.
- Understood complex entity relationships using Entity Framework Core.
- Learned JWT authentication and refresh token management.
- Implemented resource-based authorization.
- Understood real-world registration and approval workflows.
- Learned global exception handling and DTO validation.
- Gained experience writing service-level unit tests using xUnit.
- Understood frontend and backend integration using CORS.

## Status
Completed