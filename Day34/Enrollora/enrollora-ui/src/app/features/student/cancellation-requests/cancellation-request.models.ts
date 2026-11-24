
import { PagedResult } from '../../../shared/models/paged-result.model';

export enum CancellationStatus {
  Pending = 0,
  Approved = 1,
  Rejected = 2,
}

export interface CancellationRequestResponse {
  cancellationRequestId: number;
  enrollmentId: number;
  studentId: number;
  studentName: string;
  studentEmail: string;
  courseName: string;
  reason: string;
  status: CancellationStatus;
  requestedAt: string;
  reviewedAt: string | null;
  adminRemarks: string | null;
}

export interface CreateCancellationRequest {
  reason: string;
}

export interface CancellationRequestFilter {
  search?: string;
  courseId?: number | null;
  status?: CancellationStatus | null;
  fromDate?: string;
  toDate?: string;
}

export type CancellationRequestPagedResult =
  PagedResult<CancellationRequestResponse>;
