import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { StudentMarks } from './components/student-marks/student-marks';

@Component({
  imports: [RouterOutlet,StudentMarks],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  protected readonly title = signal('StudentMarksApp');
}
