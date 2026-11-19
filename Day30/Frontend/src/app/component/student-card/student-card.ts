import { Component, input } from '@angular/core';
import { StudentRecord } from '../../services/student';
import { UpperCasePipe, CurrencyPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'app-student-card',
  imports: [UpperCasePipe, CurrencyPipe, DatePipe],
  templateUrl: './student-card.html',
  styleUrl: './student-card.css',
})
export class StudentCard {

  student = input.required<StudentRecord>();

}