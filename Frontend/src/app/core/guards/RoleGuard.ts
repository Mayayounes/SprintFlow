import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router } from '@angular/router';
import { Auth } from '../services/Auth/auth';

@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {

  constructor(private router: Router, private auth: Auth) { }

  canActivate(route: ActivatedRouteSnapshot): boolean {
    const userRole = this.auth.getRole();
    const expectedRole = route.data['role'];

    if (userRole !== expectedRole) {
      this.router.navigate(['/']);
      return false;
    }
    return true;
  }
}
