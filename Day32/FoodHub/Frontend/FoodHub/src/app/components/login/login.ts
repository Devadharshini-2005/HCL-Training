
import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  private http = inject(HttpClient);
  private cdr = inject(ChangeDetectorRef);

  user = {
    email: '',
    password: ''
  };

  isLoading = false;
  successMessage = '';
  errorMessage = '';

  login(): void {

    this.successMessage = '';
    this.errorMessage = '';

    this.isLoading = true;

    this.http.post<{ message: string }>(
      'https://localhost:7180/api/auth/login',
      this.user
    ).subscribe({

      next: (response) => {

        this.successMessage = response.message;

        this.isLoading = false;
        this.cdr.detectChanges();
      },

      error: (error) => {

        console.error('Login Error:', error);

        this.errorMessage =
          error.error?.message ||
          'Unable to login. Please try again.';

        this.isLoading = false;
        this.cdr.detectChanges();
      }

    });
  }
}
