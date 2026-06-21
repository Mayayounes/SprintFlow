import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { NotificationSignalRService } from '../../../core/services/notification/notification-signal-rservice';

@Component({
  selector: 'app-main-component',
  imports: [CommonModule , RouterOutlet],
  templateUrl: './main-component.html',
  styleUrl: './main-component.css',
})
export class MainComponent {

  email = localStorage.getItem('email');
  role = localStorage.getItem('role');

  constructor(private router: Router , private notificationSignalRService: NotificationSignalRService) {}

  async logout() {
    await this.notificationSignalRService.stopConnection();
    localStorage.clear();
    this.router.navigate(['/auth']);
  }
}
