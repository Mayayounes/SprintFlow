import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { NotificationDto } from '../../../core/services/notification/notification.model';
import { Api } from '../../../core/services/api/api';
import { NotificationSignalRService } from '../../../core/services/notification/notification-signal-rservice';
import { HostListener } from '@angular/core';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notifications.html',
  styleUrls: ['./notifications.css']
})
export class Notifications implements OnInit {

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (!target.closest('.notification-wrapper')) {
      this.showDropdown = false;
    }
  }

  constructor(private api: Api, private signalR: NotificationSignalRService, private cdr: ChangeDetectorRef) { }

  notifications: NotificationDto[] = [];
  showDropdown = false;
  unreadCount = 0;
  hasNewNotification = false;

  ngOnInit(): void {

    this.loadNotifications();

    this.signalR.notificationCount$
      .subscribe(count => {
        this.unreadCount = count;
      });

    this.signalR.hasNewNotification$
      .subscribe(flag => {
        console.log('DOT FLAG:', flag);
        this.hasNewNotification = flag;
        this.cdr.detectChanges();

      });

    this.signalR.notificationReceived$
      .subscribe((data: NotificationDto) => {
        this.notifications.unshift(data);
        this.cdr.detectChanges();
      });
  }

  toggleDropdown() {
    this.showDropdown = !this.showDropdown;

    if (this.showDropdown) {
      this.signalR.clearNewNotificationFlag();
    }
  }

  loadNotifications() {

    this.api.getNotifications()
      .subscribe((res: any) => {

        this.notifications = res;
      });
  }

  markAsRead(notification: NotificationDto) {

    if (notification.isRead) return;

    this.api.markNotificationRead(notification.id)
      .subscribe({
        next: () => {

          this.notifications = this.notifications.map(n =>
            n.id === notification.id
              ? { ...n, isRead: true }
              : n
          );

          this.signalR.loadInitialCount();
        }
      });
  }
  markAllRead() {
    this.api.markAllNotificationsRead()
      .subscribe(() => {

        this.notifications = this.notifications.map(n => ({
          ...n,
          isRead: true
        }));

        this.signalR.loadInitialCount();
      });
  }
}
