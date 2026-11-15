
import { Component } from '@angular/core';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {

  name: string = 'Devadharshini S';

  role: string = 'Java Full Stack Developer';

  description: string =
    'I am a Computer Science Engineering student passionate about building responsive web applications and learning new technologies.';

  skills: string[] = [
    'Java',
    'C#',
    'Angular',
    'TypeScript',
    'HTML',
    'CSS',
    'JavaScript',
    'React',
    'Spring Boot',
    'MySQL',
    'MongoDB'
  ];

}
