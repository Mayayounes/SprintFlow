import { Injectable, NgZone } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { Subject } from 'rxjs';
import { NotificationDto } from './notification.model';
import { Api } from '../api/api';
import { Auth } from '../Auth/auth';
import { BehaviorSubject } from 'rxjs';
@Injectable({ providedIn: 'root' })
export class NotificationSignalRService {

  private hubConnection!: signalR.HubConnection;
  //notification
  private notificationSubject = new Subject<NotificationDto>();
  notificationReceived$ = this.notificationSubject.asObservable();
  //count
  private countSubject = new BehaviorSubject<number>(0);
  notificationCount$ = this.countSubject.asObservable();
  //read
  private readSubject = new Subject<NotificationDto>();
  notificationRead$ = this.readSubject.asObservable();
  // all read
  private allReadSubject = new Subject<void>();
  notificationAllRead$ = this.allReadSubject.asObservable();

  constructor(
    private api: Api,
    private zone: NgZone,
    private auth: Auth
  ) { }
  private connecting = false;
  async startConnection() {

    const token = this.auth.getToken();
    if (!token) return;

    if (
      this.connecting ||
      (this.hubConnection &&
        this.hubConnection.state !== signalR.HubConnectionState.Disconnected)
    ) {
      return;
    }

    this.connecting = true;
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.api.hubUrl}/notificationHub`, {
        accessTokenFactory: () => this.auth.getToken()
      })
      .withAutomaticReconnect()
      .build();
    this.hubConnection.on('ReceiveNotification', (data: NotificationDto) => {
      console.log('Receive Notification:', data)
      this.zone.run(() => {
        this.notificationSubject.next(data);
        this.countSubject.next(data.unreadCount);
      });
    });

    this.hubConnection.on('UnreadCountUpdated', (count: number) => {
      this.zone.run(() => {
        this.countSubject.next(count);
      });
    });

    this.hubConnection.on('NotificationRead', (data: any) => {
      this.zone.run(() => {
        this.countSubject.next(data.unreadCount);
        this.readSubject.next(data.notification);
      });
    });

    this.hubConnection.on('AllNotificationsRead', () => {
      this.zone.run(() => {
        this.allReadSubject.next();
      });
    });
    try {
      await this.hubConnection.start();

      const count =
        await this.hubConnection.invoke<number>('GetUnreadCount');

      this.countSubject.next(count);
    } finally {
      this.connecting = false;
    }
  }
  async stopConnection() {
    if (this.hubConnection) {
      await this.hubConnection.stop();
      console.log('SignalR disconnected for user:', localStorage.getItem('email'));
      this.hubConnection = undefined as any;
    }
    this.countSubject.next(0);

    this.connecting = false;
  }
  async markAsRead(notificationId: string) {
    await this.hubConnection.invoke('MarkAsRead', notificationId);
  }

  async markAllRead() {
    await this.hubConnection.invoke('MarkAllAsRead');
  }
}
