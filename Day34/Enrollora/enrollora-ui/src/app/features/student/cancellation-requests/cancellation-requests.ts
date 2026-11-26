
import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { CancellationRequestService } from './cancellation-request.service';

import {
  CancellationRequestResponse,
  CancellationRequestFilter,
  CancellationStatus,
} from './cancellation-request.models';

import { CourseResponse } from '../../admin/courses/course.models';
import { CourseService } from '../../admin/courses/course.service';

@Component({
  selector: 'app-cancellation-requests',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cancellation-requests.html',
  styleUrl: './cancellation-requests.css',
})
export class CancellationRequests implements OnInit {
  private readonly cancellationService = inject(CancellationRequestService);

  private readonly courseService = inject(CourseService);

courses = signal<CourseResponse[]>([]);

searchTerm = signal('');
selectedCourseId = signal<number | null>(null);
selectedStatus = signal<CancellationStatus | null>(null);
fromDate = signal('');
toDate = signal('');

  requests = signal<CancellationRequestResponse[]>([]);
  isLoading = signal(false);
  errorMessage = signal('');
  successMessage = signal('');

  currentPage = signal(1);
  pageSize = 10;
  totalPages = signal(0);
  totalRecords = signal(0);

  isSubmitting = signal(false);

  selectedRequest = signal<CancellationRequestResponse | null>(null);
  showDetails = signal(false);
  showRequestForm = signal(false);

  // Request form fields
  enrollmentId = signal<number | null>(null);
  reason = signal('');

  ngOnInit(): void {
  this.loadCourses();
  this.loadRequests();
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

  loadRequests(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    const filters: CancellationRequestFilter = {
  search: this.searchTerm().trim() || undefined,
  courseId: this.selectedCourseId() ?? undefined,
  status: this.selectedStatus() ?? undefined,
  fromDate: this.fromDate() || undefined,
  toDate: this.toDate() || undefined,
};

this.cancellationService
  .getMyCancellationRequests(
    this.currentPage(),
    this.pageSize,
    filters,
  ).subscribe({
        next: (response) => {
          this.requests.set(response.items);
          this.totalPages.set(response.totalPages);
          this.totalRecords.set(response.totalRecords);
          this.isLoading.set(false);
        },
        error: (error) => {
          console.error('Error loading cancellation requests:', error);
          this.errorMessage.set(
            error.error?.message || 'Unable to load cancellation requests.',
          );
          this.isLoading.set(false);
        },
      });
  }

  // Open request form
  openRequestForm(enrollmentId?: number): void {
    this.enrollmentId.set(enrollmentId ?? null);
    this.reason.set('');
    this.errorMessage.set('');
    this.showRequestForm.set(true);
  }

  // Close request form
  closeRequestForm(): void {
    this.showRequestForm.set(false);
    this.enrollmentId.set(null);
    this.reason.set('');
  }

  // Submit request
  submitCancellationRequest(): void {
  const id = this.enrollmentId();
  const requestReason = this.reason().trim();

  if (id === null) {
    this.errorMessage.set('Enrollment ID is required.');
    return;
  }

  if (!requestReason) {
    this.errorMessage.set('Please enter a cancellation reason.');
    return;
  }

  if (requestReason.length > 1000) {
    this.errorMessage.set('Reason cannot exceed 1000 characters.');
    return;
  }

  this.errorMessage.set('');
  this.isSubmitting.set(true);

  this.cancellationService
    .createCancellationRequest(id, { reason: requestReason })
    .subscribe({
      next: () => {
        this.successMessage.set(
          'Cancellation request submitted successfully.',
        );

        this.isSubmitting.set(false);
        this.closeRequestForm();
        this.currentPage.set(1);
        this.loadRequests();
      },
      error: (error) => {
        this.errorMessage.set(
          error.error?.message || 'Unable to submit cancellation request.',
        );
        this.isSubmitting.set(false);
      },
    });
}
  viewDetails(request: CancellationRequestResponse): void {
  console.log('View Details button clicked');
  console.log('Selected request:', request);

  this.selectedRequest.set(request);
  this.showDetails.set(true);

  console.log('Modal visibility:', this.showDetails());
}

  closeDetails(): void {
    this.showDetails.set(false);
    this.selectedRequest.set(null);
  }

  getStatusLabel(status: CancellationStatus): string {
    switch (status) {
      case CancellationStatus.Pending:
        return 'Pending';
      case CancellationStatus.Approved:
        return 'Approved';
      case CancellationStatus.Rejected:
        return 'Rejected';
      default:
        return 'Unknown';
    }
  }

  getStatusClass(status: CancellationStatus): string {
    switch (status) {
      case CancellationStatus.Pending:
        return 'pending';
      case CancellationStatus.Approved:
        return 'approved';
      case CancellationStatus.Rejected:
        return 'rejected';
      default:
        return '';
    }
  }

  changePage(page: number): void {
    if (page < 1 || page > this.totalPages()) {
      return;
    }

    this.currentPage.set(page);
    this.loadRequests();
  }

  previousPage(): void {
    this.changePage(this.currentPage() - 1);
  }

  nextPage(): void {
    this.changePage(this.currentPage() + 1);
  }

  onSearchChange(value: string): void {
  this.searchTerm.set(value);
  this.currentPage.set(1);
  this.loadRequests();
}

onCourseChange(value: string): void {
  this.selectedCourseId.set(
    value === '' ? null : Number(value),
  );

  this.currentPage.set(1);
  this.loadRequests();
}

onStatusChange(value: string): void {
  this.selectedStatus.set(
    value === ''
      ? null
      : Number(value) as CancellationStatus,
  );

  this.currentPage.set(1);
  this.loadRequests();
}

onFromDateChange(value: string): void {
  this.fromDate.set(value);
  this.currentPage.set(1);
  this.loadRequests();
}

onToDateChange(value: string): void {
  this.toDate.set(value);
  this.currentPage.set(1);
  this.loadRequests();
}

clearFilters(): void {
  this.searchTerm.set('');
  this.selectedCourseId.set(null);
  this.selectedStatus.set(null);
  this.fromDate.set('');
  this.toDate.set('');

  this.currentPage.set(1);
  this.loadRequests();
}

}
