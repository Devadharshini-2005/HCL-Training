
import { Component } from '@angular/core';

@Component({
  selector: 'app-about',
  standalone: true,
  imports: [],
  templateUrl: './about.html',
  styleUrl: './about.css'
})
export class About {

  name: string = 'Devadharshini S';

  role: string = 'Computer Science Engineering Student';

  introduction: string =
    'I am a passionate Computer Science Engineering student interested in software development and modern web technologies.';

  careerObjective: string =
    'My goal is to become a skilled software developer by building practical applications, improving my technical knowledge, and solving real-world problems.';

  education = {
    degree: 'B.E. Computer Science and Engineering',
    college: 'PSNA College of Engineering and Technology',
    location: 'Dindigul, Tamil Nadu',
    duration: '2023 - 2027',
    cgpa: '8.39'
  };

  interests: string[] = [
    'Full Stack Development',
    'Web Development',
    'UI/UX Design',
    'Problem Solving'
  ];

}
