
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { PagedResult } from '../../../shared/models/paged-result.model';

import {
  CancellationRequestFilter,
  CancellationRequestResponse,
  CreateCancellationRequest,
} from './cancellation-request.models';

@Injectable({
  providedIn: 'root',
})
export class CancellationRequestService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    `${environment.apiUrl}/cancellation-requests`;

  // Get cancellation requests submitted by the logged-in student
  getMyCancellationRequests(
    pageNumber = 1,
    pageSize = 10,
    filters?: CancellationRequestFilter,
  ): Observable<PagedResult<CancellationRequestResponse>> {
    return this.http.get<PagedResult<CancellationRequestResponse>>(
      `${this.apiUrl}/my`,
      {
        params: this.buildParams(pageNumber, pageSize, filters),
      },
    );
  }

  // Submit a cancellation request for an enrollment
  createCancellationRequest(
    enrollmentId: number,
    request: CreateCancellationRequest,
  ): Observable<CancellationRequestResponse> {
    return this.http.post<CancellationRequestResponse>(
      `${environment.apiUrl}/enrollments/${enrollmentId}/cancellation-request`,
      request,
    );
  }

  // Build query parameters
  private buildParams(
    pageNumber: number,
    pageSize: number,
    filters?: CancellationRequestFilter,
  ): HttpParams {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (filters?.search) {
      params = params.set('search', filters.search);
    }

    if (filters?.courseId != null) {
      params = params.set('courseId', filters.courseId);
    }

    if (filters?.status != null) {
      params = params.set('status', filters.status);
    }

    if (filters?.fromDate) {
      params = params.set('fromDate', filters.fromDate);
    }

    if (filters?.toDate) {
      params = params.set('toDate', filters.toDate);
    }

    return params;
  }
}
