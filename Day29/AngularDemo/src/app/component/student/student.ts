import { Component } from '@angular/core';

@Component({
  imports: [],
  selector: 'app-student',
  standalone : true,
  styleUrl: './student.css',
  templateUrl: './student.html',
})
export class Student {

  studentName: string = "Devadharshini";
  studentAge:number = 21;
  course: string = "CSE";

  displayDetails(){
    console.log("Student Name : ",this.studentName);
    console.log("Student Age : ",this.studentAge);
    console.log("Course : ",this.course);
  }

}
