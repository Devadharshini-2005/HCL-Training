
import { Injectable } from '@angular/core';

// Structure of a student record
export interface StudentRecord {
  name: string,
  age: number,
  email: string,
  course: string,
  date : string,
  amount : number
}

@Injectable({
  providedIn: 'root'
})
export class StudentService {

  // Common collection of student records
  students: StudentRecord[] = [];

  // Add a student to the collection
  addStudent(student: StudentRecord): void {
    this.students.push(student);
  }

  // Return all student records
  getStudents(): StudentRecord[] {
    return this.students;
  }

}
