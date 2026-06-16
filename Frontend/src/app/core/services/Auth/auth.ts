import { Injectable } from '@angular/core';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  getToken() {
    return localStorage.getItem('token') || '';
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('userId');
    localStorage.removeItem('email');
    localStorage.removeItem('role');
  }
  isTokenExpired(): boolean {
    const token = this.getToken();

    if (!token) return true;

    try {
      const decoded: any = jwtDecode(token);

      if (!decoded.exp) return true;

      return decoded.exp * 1000 < Date.now();
    }
    catch {
      return true;
    }
  }
  getRole(): string {
    return localStorage.getItem('role') || '';
  }

  isLoggedIn(): boolean {
    return !!this.getToken() && !!this.getRole() && !this.isTokenExpired();;
  }

  isAdmin(): boolean {
    return this.getRole().toLowerCase() === 'admin';
  }

  isLeader(): boolean {
    return this.getRole().toLowerCase() === 'leader';
  }

  isEmployee(): boolean {
    return this.getRole().toLowerCase() === 'employee';
  }

}
