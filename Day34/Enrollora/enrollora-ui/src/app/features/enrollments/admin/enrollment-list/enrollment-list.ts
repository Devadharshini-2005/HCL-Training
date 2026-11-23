
import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { EnrollmentService } from '../../enrollment.service';
import { EnrollmentFilter, EnrollmentResponse } from '../../enrollment.models';

import { StudentService } from '../../../admin/students/student.service';
import { StudentResponse } from '../../../admin/students/student.models';

import { CourseService } from '../../../admin/courses/course.service';
import { CourseResponse } from '../../../admin/courses/course.models';

@Component({
  selector: 'app-enrollment-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './enrollment-list.html',
  styleUrl: './enrollment-list.css',
})
export class EnrollmentListComponent implements OnInit {
  private enrollmentService = inject(EnrollmentService);
  private studentService = inject(StudentService);
  private courseService = inject(CourseService);

  enrollments = signal<EnrollmentResponse[]>([]);
  students = signal<StudentResponse[]>([]);
  courses = signal<CourseResponse[]>([]);

  selectedEnrollment = signal<EnrollmentResponse | null>(null);

  isLoading = signal(false);
  errorMessage = signal('');

  // Search and filter values
  searchText = '';
  selectedStudentId: number | null = null;
  selectedCourseId: number | null = null;
  selectedStatus: number | null = null;
  fromDate = '';
  toDate = '';

  // Pagination
  pageNumber = signal(1);
  pageSize = signal(10);
  totalRecords = signal(0);
  totalPages = signal(0);

  ngOnInit(): void {
    this.loadStudents();
    this.loadCourses();
    this.loadEnrollments();
  }

  loadStudents(): void {
    this.studentService.getStudents(1, 100).subscribe({
      next: (response) => {
        this.students.set(response.items);
      },
      error: (error) => {
        console.error('Error loading students:', error);
      },
    });
  }

  loadCourses(): void {
    this.courseService.getCourses(1, 100).subscribe({
      next: (response) => {
        this.courses.set(response.items);
      },
      error: (error) => {
        console.error('Error loading courses:', error);
      },
    });
  }

  private getFilters(): EnrollmentFilter {
    return {
      search: this.searchText.trim() || undefined,
      courseId: this.selectedCourseId,
      status: this.selectedStatus,
      fromDate: this.fromDate || undefined,
      toDate: this.toDate || undefined,
    };
  }

  loadEnrollments(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    const filters = this.getFilters();

    let request;

    // Student-specific enrollments
    if (this.selectedStudentId !== null) {
      request = this.enrollmentService.getEnrollmentsByStudent(
        this.selectedStudentId,
        this.pageNumber(),
        this.pageSize(),
        filters,
      );
    }
    // Course-specific enrollments
    else if (this.selectedCourseId !== null) {
      request = this.enrollmentService.getEnrollmentsByCourse(
        this.selectedCourseId,
        this.pageNumber(),
        this.pageSize(),
        filters,
      );
    }
    // All enrollments
    else {
      request = this.enrollmentService.getEnrollments(
        this.pageNumber(),
        this.pageSize(),
        filters,
      );
    }

    request.subscribe({
      next: (response) => {
        this.enrollments.set(response.items);
        this.totalRecords.set(response.totalRecords);
        this.totalPages.set(response.totalPages);
        this.selectedEnrollment.set(null);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading enrollments:', error);
        this.errorMessage.set('Unable to load enrollments.');
        this.isLoading.set(false);
      },
    });
  }

  applyFilters(): void {
    this.pageNumber.set(1);
    this.loadEnrollments();
  }

  clearFilters(): void {
    this.searchText = '';
    this.selectedStudentId = null;
    this.selectedCourseId = null;
    this.selectedStatus = null;
    this.fromDate = '';
    this.toDate = '';

    this.pageNumber.set(1);
    this.selectedEnrollment.set(null);

    this.loadEnrollments();
  }

  viewDetails(enrollment: EnrollmentResponse): void {
    if (this.selectedEnrollment()?.enrollmentId === enrollment.enrollmentId) {
      this.selectedEnrollment.set(null);
      return;
    }

    this.selectedEnrollment.set(enrollment);
  }

  nextPage(): void {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update((page) => page + 1);
      this.loadEnrollments();
    }
  }

  previousPage(): void {
    if (this.pageNumber() > 1) {
      this.pageNumber.update((page) => page - 1);
      this.loadEnrollments();
    }
  }
}
