import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, NgZone, OnInit } from '@angular/core';
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

  constructor(private api: Api, private signalR: NotificationSignalRService, private cdr: ChangeDetectorRef, private zone: NgZone) { }

  notifications: NotificationDto[] = [];
  showDropdown = false;
  unreadCount = 0;

  ngOnInit(): void {

    this.loadNotifications();

    this.signalR.notificationReceived$.subscribe(n => {
      this.notifications = [n, ...this.notifications];
    });
    this.signalR.notificationCount$.subscribe(count => {
      this.unreadCount = count;

      this.zone.run(() => {
        this.cdr.markForCheck();
      });
    });
    this.signalR.notificationRead$.subscribe(notificationId => {
      this.notifications = this.notifications.map(n =>
        n.id === notificationId
          ? { ...n, isRead: true }
          : n
      );
    });

    this.signalR.notificationAllRead$.subscribe(() => {
      this.notifications = this.notifications.map(n => ({
        ...n,
        isRead: true
      }));
    });
  }
  toggleDropdown() {
    this.showDropdown = !this.showDropdown;

    if (this.showDropdown) {
      console.log('🔔 opening dropdown, clearing UI indicator');
    }
    this.cdr.detectChanges();
  }

  loadNotifications() {

    this.api.getNotifications()
      .subscribe((res: any) => {

        this.notifications = res;
      });
  }
  async markAsRead(notification: NotificationDto) {
    console.log('➡️ markAsRead', notification.id);

    await this.signalR.markAsRead(notification.id);
  }
  async markAllRead() {
    console.log('➡️ markAllRead');

    await this.signalR.markAllRead();
  }
  get hasNewNotification(): boolean {
    return this.unreadCount > 0;
  }
  trackByNotification(_: number, notification: NotificationDto) {
    return notification.id;
  }
}
