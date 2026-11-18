
import {
  Component,
  inject,
  ChangeDetectorRef
} from '@angular/core';

import {
  HttpErrorResponse
} from '@angular/common/http';

import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login {

  private authService = inject(AuthService);

  private cdr = inject(ChangeDetectorRef);

  private router = inject(Router);

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

    this.authService.login(this.user).subscribe({

      next: (response) => {

        this.successMessage = response.message;

        this.isLoading = false;

        this.cdr.detectChanges();

        // Navigate to Home after successful login
        this.router.navigate(['/']);
      },

      error: (error: HttpErrorResponse) => {

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
