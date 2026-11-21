import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { CourseService } from '../course.service';
import { CourseResponse } from '../course.models';

@Component({
  selector: 'app-course-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './course-list.html',
  styleUrl: './course-list.css',
})
export class CourseListComponent implements OnInit {

  private courseService = inject(CourseService);

  // Course data
  courses = signal<CourseResponse[]>([]);

  // Loading and error states
  isLoading = signal(false);
  errorMessage = signal('');

  // Pagination
  pageNumber = signal(1);
  pageSize = signal(10);

  totalRecords = signal(0);
  totalPages = signal(0);

  // Add Course form state
  showAddCourseForm = false;
  isSaving = false;
  formError = '';

  // Form data (matches API request)
  newCourse = {
    courseName: '',
    duration: '',
    fees: 0,
    capacity: 0,
    isActive: true
  };

  ngOnInit(): void {
    this.loadCourses();
  }

  // Load courses from API
  loadCourses(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.courseService.getCourses(this.pageNumber(), this.pageSize()).subscribe({
      next: (response) => {
        this.courses.set(response.items ?? []);
        this.totalRecords.set(response.totalRecords);
        this.totalPages.set(response.totalPages);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading courses:', error);
        this.errorMessage.set('Unable to load courses.');
        this.isLoading.set(false);
      }
    });
  }

  // Pagination
  nextPage(): void {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update(page => page + 1);
      this.loadCourses();
    }
  }

  previousPage(): void {
    if (this.pageNumber() > 1) {
      this.pageNumber.update(page => page - 1);
      this.loadCourses();
    }
  }

  // Open Add Course modal
  openAddCourse(): void {
    this.formError = '';
    this.showAddCourseForm = true;
  }

  // Close modal
  closeAddCourse(): void {
    if (this.isSaving) return;

    this.showAddCourseForm = false;
    this.formError = '';
    this.resetCourseForm();
  }

  // Reset form
  resetCourseForm(): void {
    this.newCourse = {
      courseName: '',
      duration: '',
      fees: 0,
      capacity: 0,
      isActive: true
    };
  }

  // Save course
  saveCourse(): void {
    if (this.isSaving) return;

    this.formError = '';

    // Validation
    if (
      !this.newCourse.courseName.trim() ||
      !this.newCourse.duration.trim() ||
      !Number.isFinite(Number(this.newCourse.fees)) ||
      Number(this.newCourse.fees) < 0 ||
      !Number.isFinite(Number(this.newCourse.capacity)) ||
      Number(this.newCourse.capacity) <= 0
    ) {
      this.formError = 'Please enter valid course details.';
      return;
    }

    this.isSaving = true;

    this.courseService.createCourse(this.newCourse).subscribe({
      next: () => {
        this.isSaving = false;
        this.showAddCourseForm = false;

        this.resetCourseForm();

        // Refresh the course list from the first page
        this.pageNumber.set(1);
        this.loadCourses();
      },
      error: (error) => {
        console.error('Error creating course:', error);

        this.isSaving = false;

        this.formError =
          error?.error?.message ||
          error?.error?.title ||
          'Failed to create course. Please try again.';
      }
    });
  }
}