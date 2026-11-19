
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { StudentService } from '../../services/student';
import { StudentRecord } from '../../services/student';

@Component({
  selector: 'app-student',
  imports: [FormsModule],
  templateUrl: './student.html',
  styleUrl: './student.css',
})
export class Student {

  studentName: string = '';
  studentAge: number = 0;
  studentEmail: string = '';
  studentCourse: string = '';

  constructor(
    private router: Router,
    private studentService: StudentService
  ) {}

  addStudent() {

    const student: StudentRecord = {
      name: this.studentName,
      age: this.studentAge,
      email: this.studentEmail,
      course: this.studentCourse
    };

    // Store the student using the service
    this.studentService.addStudent(student);

    // Navigate to student list
    this.router.navigate(['/student-list']);

  }

}
