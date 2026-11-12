import { Component, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Student } from './component/student/student';

@Component({
  imports: [RouterOutlet, RouterLink, RouterLinkActive, Student],
  selector: 'app-root',
  standalone : true,
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('AngularDemo');
}
