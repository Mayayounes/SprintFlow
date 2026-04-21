import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class Auth {
  getToken() {
    return localStorage.getItem('token');
  }

  getRole(): string {
    return localStorage.getItem('role') || '';
  }

  isLoggedIn(): boolean {
    return !!this.getToken() && !!this.getRole();
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
