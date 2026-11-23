import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import { StudentService } from '../../admin/students/student.service';
import { StudentResponse, UpdateStudentRequest } from '../../admin/students/student.models';

@Component({
  selector: 'app-student-profile',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css',
})
export class Profile implements OnInit {
  private studentService = inject(StudentService);

  profile: StudentResponse | null = null;

  isLoading = false;
  isEditing = false;
  isSaving = false;

  errorMessage = '';
  successMessage = '';

  formError = '';

  editProfile: UpdateStudentRequest = {
    name: '',
    email: '',
    department: '',
    age: 0,
    cgpa: 0,
  };

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.studentService.getMyProfile().subscribe({
      next: (response) => {
        this.profile = response;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading profile:', error);
        this.errorMessage =
          error?.error?.message || 'Unable to load your profile.';
        this.isLoading = false;
      },
    });
  }

  startEditing(): void {
    if (!this.profile) return;

    this.editProfile = {
      name: this.profile.name,
      email: this.profile.email,
      department: this.profile.department,
      age: this.profile.age,
      cgpa: this.profile.cgpa,
    };

    this.formError = '';
    this.successMessage = '';
    this.isEditing = true;
  }

  cancelEditing(): void {
    if (this.isSaving) return;

    this.isEditing = false;
    this.formError = '';
  }

  saveProfile(): void {
    if (this.isSaving) return;

    this.formError = '';
    this.successMessage = '';

    if (
      !this.editProfile.name.trim() ||
      !this.editProfile.email.trim() ||
      !this.editProfile.department.trim() ||
      this.editProfile.age <= 0 ||
      this.editProfile.cgpa < 0 ||
      this.editProfile.cgpa > 10
    ) {
      this.formError = 'Please enter valid profile details.';
      return;
    }

    const request: UpdateStudentRequest = {
      name: this.editProfile.name.trim(),
      email: this.editProfile.email.trim(),
      department: this.editProfile.department.trim(),
      age: Number(this.editProfile.age),
      cgpa: Number(this.editProfile.cgpa),
    };

    this.isSaving = true;

    this.studentService.updateMyProfile(request).subscribe({
      next: (response) => {
        this.profile = response;
        this.isSaving = false;
        this.isEditing = false;
        this.successMessage = 'Profile updated successfully.';
      },
      error: (error) => {
        console.error('Error updating profile:', error);
        this.isSaving = false;
        this.formError =
          error?.error?.message ||
          error?.error?.title ||
          'Failed to update profile. Please try again.';
      },
    });
  }
}