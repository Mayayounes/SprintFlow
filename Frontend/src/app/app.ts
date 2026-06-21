import { Component, OnInit, signal } from '@angular/core';
import { RouterOutlet, RouterModule } from '@angular/router';
import { Toast } from './shared/components/toast/toast';
import { ErrorModalComponent } from './shared/components/error-modal/error-modal';
import { NotificationSignalRService } from './core/services/notification/notification-signal-rservice';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, Toast, ErrorModalComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App{
  protected readonly title = signal('sprintFlow');
}
