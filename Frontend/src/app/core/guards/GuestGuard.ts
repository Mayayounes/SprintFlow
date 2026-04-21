import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { Auth } from '../services/Auth/auth';
import { ROLE_ROUTES } from './auth.util';

@Injectable({ providedIn: 'root' })
export class GuestGuard implements CanActivate {

  constructor(private router: Router, private auth: Auth) { }

  canActivate(): boolean {

    const role = this.auth.getRole();
    const isLoggedIn = this.auth.isLoggedIn();
    if (isLoggedIn) {
      const targetRoute = ROLE_ROUTES[role as string];
      this.router.navigateByUrl(targetRoute || '/', { replaceUrl: true });
      return false;
    }
    return true;
  }
}
