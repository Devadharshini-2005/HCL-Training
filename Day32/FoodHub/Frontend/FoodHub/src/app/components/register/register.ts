
import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {

  private http = inject(HttpClient);
  private cdr = inject(ChangeDetectorRef);

  user = {
    fullName: '',
    email: '',
    password: ''
  };

  isLoading = false;
  successMessage = '';
  errorMessage = '';

  register(): void {

    this.successMessage = '';
    this.errorMessage = '';

    this.isLoading = true;

    this.http.post<{ message: string }>(
      'https://localhost:7180/api/auth/register',
      this.user
    ).subscribe({

      next: (response) => {

        this.successMessage = response.message;

        this.user = {
          fullName: '',
          email: '',
          password: ''
        };

        this.isLoading = false;
        this.cdr.detectChanges();
      },

      error: (error) => {

        console.error('Registration Error:', error);

        this.errorMessage =
          error.error?.message ||
          'Unable to register. Please try again.';

        this.isLoading = false;
        this.cdr.detectChanges();
      }

    });
  }
}
