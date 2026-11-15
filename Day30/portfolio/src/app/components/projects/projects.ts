
import { Component } from '@angular/core';

@Component({
  selector: 'app-projects',
  standalone: true,
  imports: [],
  templateUrl: './projects.html',
  styleUrl: './projects.css'
})
export class Projects {

  projectList = [
    {
      title: 'Movie Ticket Booking System',

      description:
        'A movie ticket booking application that allows customers to explore movies, view shows, select seats and manage their bookings. Includes authentication and role-based authorization.',

      technologies: [
        'C#',
        'ASP.NET Core',
        'Entity Framework Core',
        'SQL Server',
        'JWT'
      ],

      status: 'Completed',

      githubUrl: ''
    },
    {
      title: 'Campus Placement Management System',

      description:
        'A web-based application designed to manage campus placement activities, including student information, companies, job applications, recruitment drives and interviews.',

      technologies: [
        'C#',
        'ASP.NET Core',
        'Entity Framework Core',
        'SQL Server'
      ],

      status: 'In Progress',

      githubUrl: ''
    },
    {
      title: 'Online Learning Platform',

      description:
        'A learning platform where students can register, log in, explore courses, view course details and access learning content. Includes student and admin functionality.',

      technologies: [
        'React',
        'Node.js',
        'Express.js',
        'MongoDB',
        'JavaScript'
      ],

      status: 'Completed',

      githubUrl: ''
    }
  ];

}
