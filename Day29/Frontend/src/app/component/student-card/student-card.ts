import { Component, input } from '@angular/core';
import { StudentRecord } from '../../services/student';

@Component({
  selector: 'app-student-card',
  imports: [],
  templateUrl: './student-card.html',
  styleUrl: './student-card.css',
})
export class StudentCard {

  student = input.required<StudentRecord>();

}