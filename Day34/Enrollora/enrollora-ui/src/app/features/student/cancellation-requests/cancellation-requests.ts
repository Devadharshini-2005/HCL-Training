
import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { CancellationRequestService } from './cancellation-request.service';
import {
  CancellationRequestResponse,
  CancellationStatus,
} from './cancellation-request.models';

@Component({
  selector: 'app-cancellation-requests',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './cancellation-requests.html',
  styleUrl: './cancellation-requests.css',
})
export class CancellationRequests implements OnInit {
  private readonly cancellationService = inject(CancellationRequestService);

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
    this.loadRequests();
  }

  loadRequests(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.cancellationService
      .getMyCancellationRequests(this.currentPage(), this.pageSize)
      .subscribe({
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
}
