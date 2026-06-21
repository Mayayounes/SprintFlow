import { Component } from '@angular/core';
import { Api } from '../../core/services/api/api';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from "@angular/router";
import { ToastService } from '../../core/services/toast/toast';
import { ErrorModalService } from '../../core/services/error-modal/error-modal';
import { ROLE_ROUTES } from '../../core/guards/auth.util';
import { NotificationSignalRService } from '../../core/services/notification/notification-signal-rservice';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './login.html',
})
export class Login {

  email = '';
  password = '';
  isLoginMode = true;

  constructor(
    private api: Api,
    private router: Router,
    private toast: ToastService,
    private errorModal: ErrorModalService,
    private notificationSignalRService: NotificationSignalRService
  ) { }

  login() {
    const data = {
      email: this.email,
      password: this.password,
    };

    this.api.login(data).subscribe({
      next:async(res: any) => {
        if (!res?.data?.token || !res?.data?.role) {
          this.errorModal.show('Invalid response from server');
          return;
        }

        const token = res.data.token;
        const roleRaw = res.data.role;
        const role = roleRaw.toLowerCase().trim();
        const email = res.data.email;
        const userId = res.data.userId;

        localStorage.setItem('token', token);
        localStorage.setItem('role', role);
        localStorage.setItem('email', email);
        localStorage.setItem('userId', userId);

        await this.notificationSignalRService.startConnection();
        this.toast.show('Login Successful', 'success');
        const targetRoute = ROLE_ROUTES[role as string];
        if (!targetRoute) {
          console.error('Unknown role:', roleRaw);
          localStorage.clear();
          this.errorModal.show('Invalid user role');
          return;
        }
        setTimeout(() => {
          this.router.navigateByUrl(targetRoute, { replaceUrl: true });
        }, 0);
      },

      error: (err) => {
        console.error('Login Error', err);
        const backendErrors = err?.error?.errors;
        if (backendErrors) {
          this.errorModal.show(backendErrors);
        } else {
          this.errorModal.show('Invalid email or password');
        }
      },
    });
  }

  toggle() {
    this.isLoginMode = !this.isLoginMode;
  }
}
