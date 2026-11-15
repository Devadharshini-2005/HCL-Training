
import { Component } from '@angular/core';

@Component({
  selector: 'app-skills',
  standalone: true,
  imports: [],
  templateUrl: './skills.html',
  styleUrl: './skills.css'
})
export class Skills {

  skillCategories = [
    {
      title: 'Programming Languages',
      skills: ['Java', 'C#', 'JavaScript', 'TypeScript']
    },
    {
      title: 'Frontend Technologies',
      skills: ['HTML', 'CSS', 'Angular', 'React']
    },
    {
      title: 'Backend Technologies',
      skills: ['Spring Boot', 'ASP.NET Core', 'REST API']
    },
    {
      title: 'Databases',
      skills: ['MySQL', 'MongoDB', 'SQL Server']
    },
    {
      title: 'Development Tools',
      skills: ['VS Code', 'Visual Studio', 'Eclipse', 'Git', 'GitHub']
    },
    {
      title: 'Design Tools',
      skills: ['Figma', 'Adobe XD']
    }
  ];

}
