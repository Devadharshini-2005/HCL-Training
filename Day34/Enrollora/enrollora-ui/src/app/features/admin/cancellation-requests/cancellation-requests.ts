
import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { debounceTime, distinctUntilChanged, Subject } from 'rxjs';

import { CancellationRequestService } from './cancellation-request.service';

import {
  CancellationRequestResponse,
  CancellationStatus,
  CancellationRequestFilter,
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

  searchTerm = signal('');
  selectedStatus = signal<number | null>(null);

  selectedRequest = signal<CancellationRequestResponse | null>(null);
  showDetails = signal(false);

  // Admin review
  reviewRemarks = signal('');
  isReviewing = signal(false);

  readonly CancellationStatus = CancellationStatus;

  private readonly searchSubject = new Subject<string>();

  ngOnInit(): void {
    this.loadRequests();

    this.searchSubject
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
      )
      .subscribe((search) => {
        this.searchTerm.set(search);
        this.currentPage.set(1);
        this.loadRequests();
      });
  }

  // Load all cancellation requests
  loadRequests(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    const filters: CancellationRequestFilter = {
      search: this.searchTerm().trim() || undefined,
      status: this.selectedStatus() ?? undefined,
    };

    this.cancellationService
      .getAllCancellationRequests(
        this.currentPage(),
        this.pageSize,
        filters,
      )
      .subscribe({
        next: (response) => {
  console.log('Cancellation Requests:', response);

  this.requests.set(response.items);
  this.totalPages.set(response.totalPages);
  this.totalRecords.set(response.totalRecords);
  this.isLoading.set(false);
},
        error: (error) => {
          console.error('Error loading cancellation requests:', error);

          this.errorMessage.set(
            error.error?.message ||
            'Unable to load cancellation requests.',
          );

          this.isLoading.set(false);
        },
      });
  }

  // Search
  onSearchChange(value: string): void {
    this.searchSubject.next(value);
  }

  // Filter by status
  onStatusChange(value: string): void {
    this.selectedStatus.set(
      value === '' ? null : Number(value),
    );

    this.currentPage.set(1);
    this.loadRequests();
  }

  // Clear filters
  clearFilters(): void {
    this.searchTerm.set('');
    this.selectedStatus.set(null);
    this.currentPage.set(1);
    this.loadRequests();
  }

  // View details
  viewDetails(request: CancellationRequestResponse): void {
    this.selectedRequest.set(request);
    this.reviewRemarks.set('');
    this.showDetails.set(true);
  }

  // Close details
  closeDetails(): void {
    this.showDetails.set(false);
    this.selectedRequest.set(null);
    this.reviewRemarks.set('');
  }

  // Approve or reject request
  reviewRequest(action: 'approve' | 'reject'): void {
    const request = this.selectedRequest();

    if (!request || this.isReviewing()) {
      return;
    }

    if (!this.isPending(request.status)) {
      return;
    }

    this.isReviewing.set(true);
    this.errorMessage.set('');
    this.successMessage.set('');

    const reviewData = {
      adminRemarks: this.reviewRemarks().trim() || undefined,
    };

    const operation =
      action === 'approve'
        ? this.cancellationService.approveCancellationRequest(
            request.cancellationRequestId,
            reviewData,
          )
        : this.cancellationService.rejectCancellationRequest(
            request.cancellationRequestId,
            reviewData,
          );

    operation.subscribe({
      next: (response) => {
        this.successMessage.set(
          action === 'approve'
            ? 'Cancellation request approved successfully.'
            : 'Cancellation request rejected successfully.',
        );

        this.isReviewing.set(false);
        this.closeDetails();

        // Refresh the request list with updated status
        this.loadRequests();
      },
      error: (error) => {
        console.error('Error reviewing cancellation request:', error);

        this.errorMessage.set(
          error.error?.message ||
          `Unable to ${action} the cancellation request.`,
        );

        this.isReviewing.set(false);
      },
    });
  }

  // Status label
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

  // Status CSS class
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

  // Check pending status
  isPending(status: CancellationStatus): boolean {
    return status === CancellationStatus.Pending;
  }

  // Pagination
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
