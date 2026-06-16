import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { Notifications } from '../../shared/models/notifications/notifications';
import { NotificationSignalRService } from '../../core/services/notification/notification-signal-rservice';

@Component({
  selector: 'app-leader',
  imports: [RouterOutlet, CommonModule , RouterModule , Notifications],
  templateUrl: './leader.html',
  styleUrl: './leader.css',
})
export class Leader implements OnInit{

  constructor(
    private signalR: NotificationSignalRService
  ) {}

  ngOnInit(): void {
    this.signalR.startConnection();
  }
}
