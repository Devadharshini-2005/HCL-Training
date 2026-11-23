
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { CourseResponse } from '../../../admin/courses/course.models';
import { CourseService } from '../../../admin/courses/course.service';
import { EnrollmentService } from '../../enrollment.service';

@Component({
  selector: 'app-available-courses',
  standalone: true,
  imports: [CommonModule, FormsModule],
  styleUrl: './available-courses.css',
  templateUrl: './available-courses.html',
})
export class AvailableCourses implements OnInit {
  private readonly courseService = inject(CourseService);
  private readonly enrollmentService = inject(EnrollmentService);

  courses = signal<CourseResponse[]>([]);

  isLoading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');
  enrollmentError = signal('');
  processingCourseId = signal<number | null>(null);

  // Search and sorting
  searchText = '';
  sortBy = signal('name');

  displayedCourses = computed(() => {
    const result = [...this.courses()];

    switch (this.sortBy()) {
      case 'fees-low':
        return result.sort((a, b) => a.fees - b.fees);

      case 'fees-high':
        return result.sort((a, b) => b.fees - a.fees);

      case 'popular':
        return result.sort((a, b) => b.enrolledCount - a.enrolledCount);

      default:
        return result.sort((a, b) =>
          a.courseName.localeCompare(b.courseName),
        );
    }
  });

  // Course details
  selectedCourse = signal<CourseResponse | null>(null);
  isDetailsLoading = signal(false);
  detailsError = signal('');

  // Pagination
  pageNumber = signal(1);
  pageSize = signal(9);
  totalRecords = signal(0);
  totalPages = signal(0);

  ngOnInit(): void {
    this.loadCourses();
  }

  loadCourses(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.courseService
      .getCourses(this.pageNumber(), this.pageSize(), {
        isActive: true,
        search: this.searchText.trim() || undefined,
      })
      .subscribe({
        next: (response) => {
          this.courses.set(
            response.items.filter(
              course => course.isActive && course.availableSeats > 0,
            ),
          );

          this.totalRecords.set(response.totalRecords);
          this.totalPages.set(response.totalPages);
          this.isLoading.set(false);
        },
        error: (error: HttpErrorResponse) => {
          this.errorMessage.set(
            this.getErrorMessage(error, 'Unable to load courses.'),
          );
          this.isLoading.set(false);
        },
      });
  }

  searchCourses(): void {
    this.pageNumber.set(1);
    this.loadCourses();
  }

  clearSearch(): void {
    this.searchText = '';
    this.pageNumber.set(1);
    this.loadCourses();
  }

  changeSort(value: string): void {
    this.sortBy.set(value);
  }

  viewCourseDetails(course: CourseResponse): void {
    this.selectedCourse.set(null);
    this.detailsError.set('');
    this.isDetailsLoading.set(true);

    this.courseService.getCourseById(course.courseId).subscribe({
      next: (response) => {
        this.selectedCourse.set(response);
        this.isDetailsLoading.set(false);
      },
      error: (error: HttpErrorResponse) => {
        this.detailsError.set(
          this.getErrorMessage(error, 'Unable to load course details.'),
        );
        this.isDetailsLoading.set(false);
      },
    });
  }

  closeCourseDetails(): void {
    this.selectedCourse.set(null);
    this.detailsError.set('');
  }

  enroll(course: CourseResponse): void {
    if (
      this.processingCourseId() !== null ||
      course.availableSeats <= 0
    ) {
      return;
    }

    this.processingCourseId.set(course.courseId);
    this.successMessage.set('');
    this.enrollmentError.set('');

    this.enrollmentService
      .createEnrollment({ courseId: course.courseId })
      .subscribe({
        next: () => {
          this.processingCourseId.set(null);
          this.successMessage.set(
            `You have successfully enrolled in ${course.courseName}.`,
          );
          this.loadCourses();
        },
        error: (error: HttpErrorResponse) => {
          this.processingCourseId.set(null);
          this.enrollmentError.set(
            this.getErrorMessage(error, 'Unable to enroll in this course.'),
          );
        },
      });
  }

  previousPage(): void {
    if (this.pageNumber() > 1) {
      this.pageNumber.update(page => page - 1);
      this.loadCourses();
    }
  }

  nextPage(): void {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update(page => page + 1);
      this.loadCourses();
    }
  }

  private getErrorMessage(
    error: HttpErrorResponse,
    fallback: string,
  ): string {
    const body = error.error;

    if (typeof body === 'string' && body.trim()) {
      return body;
    }

    if (body?.detail) return body.detail;
    if (body?.message) return body.message;
    if (body?.title) return body.title;

    if (body?.errors) {
      const messages = Object.values(body.errors).flat();
      if (messages.length > 0) {
        return messages.map(message => String(message)).join(' ');
      }
    }

    return fallback;
  }
}
