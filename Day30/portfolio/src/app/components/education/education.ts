
import { Component } from '@angular/core';

@Component({
  selector: 'app-education',
  standalone: true,
  imports: [],
  templateUrl: './education.html',
  styleUrl: './education.css'
})
export class Education {

  educationDetails = [
    {
      degree: 'B.E. Computer Science and Engineering',
      institution: 'PSNA College of Engineering and Technology',
      location: 'Dindigul, Tamil Nadu',
      duration: '2023 - 2027',
      score: 'CGPA: 8.39',
      status: 'Currently Pursuing'
    },
    {
      degree: 'Higher Secondary Education (Class XII)',
      institution: 'Annamalaiyar Mills Girls Higher Secondary School',
      location: 'Tamil Nadu',
      duration: '2022 - 2023',
      score: 'Percentage: 86.66%',
      status: 'Completed'
    },
    {
      degree: 'Secondary Education (Class X)',
      institution: 'Annamalaiyar Mills Girls Higher Secondary School',
      location: 'Tamil Nadu',
      duration: '2020 - 2021',
      score: 'Completed',
      status: 'Completed'
    }
  ];

}
