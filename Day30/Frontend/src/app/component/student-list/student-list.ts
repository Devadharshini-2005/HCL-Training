import { Component } from '@angular/core';
import { UpperCasePipe, CurrencyPipe, DatePipe } from '@angular/common';
import {StudentService,StudentRecord} from '../../services/student';

import { StudentCard } from '../student-card/student-card';

@Component({
  selector: 'app-student-list',
  imports: [StudentCard,UpperCasePipe,CurrencyPipe, DatePipe],
  templateUrl: './student-list.html',
  styleUrl: './student-list.css',
})
export class StudentList {

  students: StudentRecord[] = [];

  constructor(private studentService: StudentService) {
    this.students = this.studentService.getStudents();
  }

}