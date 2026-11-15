
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { Navbar } from './components/navbar/navbar';
import { About } from './components/about/about';
import { Skills } from './components/skills/skills';
import { Education } from './components/education/education';
import { Projects } from './components/projects/projects';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    Navbar,
    About,
    Skills,
    Education,
    Projects
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  title = 'MyPortfolio';
}
