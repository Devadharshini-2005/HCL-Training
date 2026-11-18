
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

interface LoginRequest {
  email: string;
  password: string;
}

interface LoginResponse {
  message: string;
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private http = inject(HttpClient);

  private apiUrl = 'https://localhost:7180/api/auth';

  private tokenKey = 'foodhub_token';

  login(credentials: LoginRequest): Observable<LoginResponse> {

    return this.http.post<LoginResponse>(
      `${this.apiUrl}/login`,
      credentials
    ).pipe(
      tap(response => {

        console.log('Login response received:', response.message);

        localStorage.setItem(this.tokenKey, response.token);

        console.log(
          'Token saved:',
          localStorage.getItem(this.tokenKey) !== null
        );

      })
    );
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn(): boolean {
    return this.getToken() !== null;
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
  }

  getUserRole(): string | null {

  const token = this.getToken();

  if (!token) {
    return null;
  }

  try {

    const payload = JSON.parse(
      atob(token.split('.')[1])
    );

    return payload[
      'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
    ] || payload['role'] || null;

  } catch {
    return null;
  }
}

}
