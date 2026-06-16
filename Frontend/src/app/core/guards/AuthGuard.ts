import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Auth } from '../services/Auth/auth';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {

  constructor(private router: Router , private auth :  Auth) {}

  canActivate(): boolean {
    const token = this.auth.getToken()
    if (!token) {
      this.router.navigate(['/auth']);
      return false;
    }

  if (this.auth.isTokenExpired()) {
    this.auth.logout();
    this.router.navigate(['/auth']);
    return false;
  }

    return true;
  }
};
