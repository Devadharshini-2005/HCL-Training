import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';
export const authGuard:CanActivateFn=()=>{const a=inject(AuthService);const r=inject(Router);if(a.current())return true;r.navigate(['/login']);return false;};
export const adminGuard:CanActivateFn=()=>{const a=inject(AuthService);const r=inject(Router);if(a.isAdmin())return true;r.navigate(['/']);return false;};
export const customerGuard:CanActivateFn=()=>{const a=inject(AuthService);const r=inject(Router);if(a.isCustomer())return true;r.navigate(['/login']);return false;};