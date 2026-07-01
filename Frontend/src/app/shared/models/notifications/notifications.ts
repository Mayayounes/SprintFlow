import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, NgZone, OnInit, HostListener } from '@angular/core';
import { NotificationDto } from '../../../core/services/notification/notification.model';
import { Api } from '../../../core/services/api/api';
import { NotificationSignalRService } from '../../../core/services/notification/notification-signal-rservice';

@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './notifications.html',
  styleUrls: ['./notifications.css']
})
export class Notifications implements OnInit {

  constructor(
    private api: Api,
    private signalR: NotificationSignalRService,
    private cdr: ChangeDetectorRef,
    private zone: NgZone
  ) { }

  notifications: NotificationDto[] = [];
  showDropdown = false;
  unreadCount = 0;

  currentPage = 1;
  pageSize = 10;
  totalItems = 0;

  selectedFilter: string = 'All';

  loadingMore = false;
  hasMore = true;

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (!target.closest('.notification-wrapper')) {
      this.showDropdown = false;
    }
  }

  ngOnInit(): void {
    this.loadNotifications();

    this.signalR.notificationReceived$.subscribe(n => {
      if (this.selectedFilter === 'Seen') return;

      this.handleRealtimeNotification(n);
    });

    this.signalR.notificationCount$.subscribe(count => {
      this.unreadCount = count;

      this.zone.run(() => {
        this.cdr.markForCheck();
      });
    });
    this.signalR.notificationRead$.subscribe(updated => {

      if (this.selectedFilter === 'Seen') {

        const exists = this.notifications.some(n => n.id === updated.id);

        if (!exists) {
          this.notifications = [updated, ...this.notifications];
        }

        return;
      }

      if (this.selectedFilter === 'Unread') {
        this.notifications =
          this.notifications.filter(n => n.id !== updated.id);
        return;
      }

      this.notifications = this.notifications.map(n =>
        n.id === updated.id
          ? updated
          : n
      );

      this.cdr.detectChanges();
    });

    this.signalR.notificationAllRead$.subscribe(() => {
      this.notifications = this.notifications.map(n => ({
        ...n,
        isRead: true
      }));
    });
  }

  private handleRealtimeNotification(n: NotificationDto) {
    if (this.selectedFilter === 'Seen') return;
    const exists = this.notifications.some(x => x.id === n.id);
    if (exists) return;
    this.notifications = [n, ...this.notifications];
    this.totalItems++;
  }

  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
    this.cdr.detectChanges();
  }
  private mapFilter(filter: string): string {
    switch (filter) {
      case 'Unread': return 'Unread';
      case 'Seen': return 'Seen';
      default: return 'All';
    }
  }

  changeFilter(filter: string) {

    this.selectedFilter = filter;
    console.log('Clicked:', filter);
    this.currentPage = 1;
    this.notifications = [];
    this.hasMore = true;

    this.loadNotifications(true);
  }
  onScroll(event: any) {
    const el = event.target;

    const scrollPosition = el.scrollTop + el.clientHeight;
    const scrollHeight = el.scrollHeight;

    const nearBottom = scrollPosition >= scrollHeight - 20;

    if (nearBottom) {
      console.log('⬇️ Bottom reached');

      this.loadMore();
    }
  }
  loadMore() {
    if (this.loadingMore || !this.hasMore) return;

    console.log('📦 Loading page:', this.currentPage + 1);

    this.loadingMore = true;
    this.currentPage++;

    this.loadNotifications(false);
  }
  loadNotifications(reset: boolean = true) {

    if (reset) {
      this.currentPage = 1;
      this.notifications = [];
      this.hasMore = true;
    }
    console.log('loading with filter', this.selectedFilter);
    console.log('page:', this.currentPage, 'filter:', this.selectedFilter);
    this.api.getNotifications(
      this.mapFilter(this.selectedFilter),
      this.currentPage,
      this.pageSize
    )
      .subscribe({
        next: (res: any) => {

          console.log('API RESPONSE', res);

          const data = res?.data;

          const newItems = data?.items ?? [];

          this.notifications = [
            ...this.notifications,
            ...newItems
          ];

          this.totalItems = data?.totalItemsCount ?? 0;
          if (newItems.length < this.pageSize) {
            this.hasMore = false;
          }

          this.loadingMore = false;
          this.cdr.detectChanges();
        },
        error: err => {
          console.error('API ERROR', err);
        }
      });
  }

  async markAsRead(notification: NotificationDto) {
    console.log('➡️ markAsRead', notification.id);
    await this.signalR.markAsRead(notification.id);
  }

  async markAllRead() {
    await this.signalR.markAllRead();
    this.loadNotifications();
  }

  get hasNewNotification(): boolean {
    return this.unreadCount > 0;
  }

  trackByNotification(_: number, notification: NotificationDto) {
    return notification.id;
  }
}
