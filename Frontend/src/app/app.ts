import { Component, OnInit, signal } from '@angular/core';
import { RouterOutlet, RouterModule } from '@angular/router';
import { Toast } from './shared/components/toast/toast';
import { ErrorModalComponent } from './shared/components/error-modal/error-modal';
import { NotificationSignalRService } from './core/services/notification/notification-signal-rservice';
import { Auth } from './core/services/Auth/auth';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, Toast, ErrorModalComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit{
  protected readonly title = signal('sprintFlow');
    constructor(
    private notificationSignalR: NotificationSignalRService,
    private auth: Auth
  ) {}

  async ngOnInit() {

    const token = this.auth.getToken();

    if (token) {
      await this.notificationSignalR.startConnection();
    }
  }
}
