
import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { Navbar } from './components/navbar/navbar';
import { About } from './components/about/about';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    Navbar,
    About
  ],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  title = 'MyPortfolio';
}
