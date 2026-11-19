
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
  date: string = new Date().toISOString().split('T')[0];
  amount : number = 0;

  constructor(
    private router: Router,
    private studentService: StudentService
  ) {}

  addStudent() {

    if(this.studentCourse === "Angular"){
      this.amount = 2000;
    }
    else if(this.studentCourse === "React"){
      this.amount = 2500;
    }
    else if(this.studentCourse === "Java"){
      this.amount = 10000;
    }
    else if(this.studentCourse === "Python"){
      this.amount = 5000;
    }
    

   const student: StudentRecord = {
  name: this.studentName,
  age: this.studentAge,
  email: this.studentEmail,
  course: this.studentCourse,
  date: this.date,
  amount: this.amount
};

    // Store the student using the service
    this.studentService.addStudent(student);

    // Navigate to student list
    this.router.navigate(['/student-list']);

  }

}
