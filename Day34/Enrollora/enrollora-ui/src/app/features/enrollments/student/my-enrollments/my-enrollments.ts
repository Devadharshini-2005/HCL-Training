
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { EnrollmentResponse } from '../../enrollment.models';
import { EnrollmentService } from '../../enrollment.service';

import { CancellationRequestService } from '../../../student/cancellation-requests/cancellation-request.service';
import {
  CancellationRequestResponse,
  CancellationStatus,
} from '../../../student/cancellation-requests/cancellation-request.models';

@Component({
  selector: 'app-my-enrollments',
  standalone: true,
  imports: [CommonModule, FormsModule],
  styleUrl: './my-enrollments.css',
  templateUrl: './my-enrollments.html',
})
export class MyEnrollments implements OnInit {
  private readonly enrollmentService = inject(EnrollmentService);
  private readonly cancellationService = inject(CancellationRequestService);

  enrollments = signal<EnrollmentResponse[]>([]);
  selectedEnrollment = signal<EnrollmentResponse | null>(null);

  isLoading = signal(false);
  errorMessage = signal('');

  pageNumber = signal(1);
  pageSize = signal(10);
  totalRecords = signal(0);
  totalPages = signal(0);

  // Cancellation request properties
  pendingEnrollmentIds = signal<number[]>([]);
  selectedCancellationEnrollment = signal<EnrollmentResponse | null>(null);

  cancellationReason = signal('');
  showCancellationForm = signal(false);
  isSubmittingCancellation = signal(false);

  cancellationError = signal('');
  cancellationSuccess = signal('');

  ngOnInit(): void {
    this.loadEnrollments();
    this.loadCancellationRequests();
  }

  loadEnrollments(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.enrollmentService
      .getMyEnrollments(this.pageNumber(), this.pageSize())
      .subscribe({
        next: (response) => {
          this.enrollments.set(response.items);
          this.totalRecords.set(response.totalRecords);
          this.totalPages.set(response.totalPages);
          this.isLoading.set(false);
        },
        error: (error: HttpErrorResponse) => {
          const body = error.error;

          this.errorMessage.set(
            body?.detail ||
            body?.message ||
            body?.title ||
            'Unable to load your enrollments.',
          );

          this.isLoading.set(false);
        },
      });
  }

  // Load pending cancellation requests
  loadCancellationRequests(): void {
    this.cancellationService
      .getMyCancellationRequests(1, 100)
      .subscribe({
        next: (response) => {
          const pendingIds = response.items
            .filter(
              (request: CancellationRequestResponse) =>
                request.status === CancellationStatus.Pending,
            )
            .map((request) => request.enrollmentId);

          this.pendingEnrollmentIds.set(pendingIds);
        },
        error: (error) => {
          console.error('Unable to load cancellation requests:', error);
        },
      });
  }

  // Enrollment status: Active = 0, Cancelled = 1
  statusLabel(status: number): string {
    return status === 0 ? 'Active' : 'Cancelled';
  }

  viewDetails(enrollment: EnrollmentResponse): void {
    this.selectedEnrollment.set(enrollment);
  }

  closeDetails(): void {
    this.selectedEnrollment.set(null);
  }

  // Check whether a pending cancellation request exists
  hasPendingRequest(enrollmentId: number): boolean {
    return this.pendingEnrollmentIds().includes(enrollmentId);
  }

  // Open cancellation form
  openCancellationForm(enrollment: EnrollmentResponse): void {
    this.selectedCancellationEnrollment.set(enrollment);
    this.cancellationReason.set('');
    this.cancellationError.set('');
    this.cancellationSuccess.set('');
    this.showCancellationForm.set(true);
  }

  // Close cancellation form
  closeCancellationForm(): void {
    this.showCancellationForm.set(false);
    this.selectedCancellationEnrollment.set(null);
    this.cancellationReason.set('');
    this.cancellationError.set('');
  }

  // Submit cancellation request
  submitCancellationRequest(): void {
  // Prevent multiple submissions
  if (this.isSubmittingCancellation()) {
    return;
  }

  const enrollment = this.selectedCancellationEnrollment();
  const reason = this.cancellationReason().trim();

  if (!enrollment) {
    this.cancellationError.set('Please select an enrollment.');
    return;
  }

  // Only active enrollments can be cancelled
  if (enrollment.status !== 0) {
    this.cancellationError.set(
      'Only active enrollments can be cancelled.',
    );
    return;
  }

  if (!reason) {
    this.cancellationError.set(
      'Please enter a cancellation reason.',
    );
    return;
  }

  if (reason.length > 1000) {
    this.cancellationError.set(
      'The reason cannot exceed 1000 characters.',
    );
    return;
  }

  if (this.hasPendingRequest(enrollment.enrollmentId)) {
    this.cancellationError.set(
      'A pending cancellation request already exists for this enrollment.',
    );
    return;
  }

  this.isSubmittingCancellation.set(true);
  this.cancellationError.set('');

  this.cancellationService
    .createCancellationRequest(enrollment.enrollmentId, { reason })
    .subscribe({
      next: () => {
        this.isSubmittingCancellation.set(false);

        this.closeCancellationForm();

        this.cancellationSuccess.set(
          'Cancellation request submitted successfully.',
        );

        // Refresh pending cancellation requests
        this.loadCancellationRequests();
      },

      error: (error: HttpErrorResponse) => {
        const body = error.error;

        this.cancellationError.set(
          body?.detail ||
          body?.message ||
          body?.title ||
          'Unable to submit cancellation request.',
        );

        this.isSubmittingCancellation.set(false);
      },
    });
}

  previousPage(): void {
    if (this.pageNumber() > 1) {
      this.pageNumber.update((page) => page - 1);
      this.loadEnrollments();
    }
  }

  nextPage(): void {
    if (this.pageNumber() < this.totalPages()) {
      this.pageNumber.update((page) => page + 1);
      this.loadEnrollments();
    }
  }
}
