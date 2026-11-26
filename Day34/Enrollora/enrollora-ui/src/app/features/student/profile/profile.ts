
import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { StudentService } from '../../admin/students/student.service';
import {
  StudentResponse,
  UpdateMyProfileRequest,
} from '../../admin/students/student.models';

@Component({
  selector: 'app-student-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile implements OnInit {
  private studentService = inject(StudentService);

  profile = signal<StudentResponse | null>(null);

  isLoading = signal(false);
  isEditing = signal(false);
  isSaving = signal(false);

  errorMessage = signal('');
  successMessage = signal('');
  formError = signal('');

  // Editable fields (email is read-only)
  editProfile = {
    name: '',
    department: '',
    age: 0,
    cgpa: 0,
  };

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.studentService.getMyProfile().subscribe({
      next: (response) => {
        this.profile.set(response);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error loading profile:', error);

        this.errorMessage.set(
          error?.error?.message || 'Unable to load your profile.'
        );

        this.isLoading.set(false);
      },
    });
  }

  startEditing(): void {
    const student = this.profile();

    if (!student) return;

    this.editProfile = {
      name: student.name,
      department: student.department,
      age: student.age,
      cgpa: student.cgpa,
    };

    this.formError.set('');
    this.successMessage.set('');
    this.isEditing.set(true);
  }

  cancelEditing(): void {
    if (this.isSaving()) return;

    this.isEditing.set(false);
    this.formError.set('');
  }

  saveProfile(): void {
    if (this.isSaving()) return;

    this.formError.set('');
    this.successMessage.set('');

    if (
      !this.editProfile.name.trim() ||
      !this.editProfile.department.trim() ||
      this.editProfile.age <= 0 ||
      this.editProfile.cgpa < 0 ||
      this.editProfile.cgpa > 10
    ) {
      this.formError.set('Please enter valid profile details.');
      return;
    }

    const request: UpdateMyProfileRequest = {
      name: this.editProfile.name.trim(),
      department: this.editProfile.department.trim(),
      age: Number(this.editProfile.age),
      cgpa: Number(this.editProfile.cgpa),
    };

    this.isSaving.set(true);

    this.studentService.updateMyProfile(request).subscribe({
      next: (response) => {
        this.profile.set(response);
        this.isSaving.set(false);
        this.isEditing.set(false);
        this.successMessage.set('Profile updated successfully.');
      },
      error: (error) => {
        console.error('Error updating profile:', error);

        this.isSaving.set(false);

        this.formError.set(
          error?.error?.message ||
          error?.error?.title ||
          'Failed to update profile. Please try again.'
        );
      },
    });
  }
}
