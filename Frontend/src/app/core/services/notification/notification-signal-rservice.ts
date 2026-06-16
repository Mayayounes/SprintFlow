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
  private started = false;

  private notificationSubject = new Subject<NotificationDto>();

  private newNotificationSubject = new BehaviorSubject<boolean>(false);
  hasNewNotification$ = this.newNotificationSubject.asObservable();
  notificationReceived$ = this.notificationSubject.asObservable();

  private countSubject = new BehaviorSubject<number>(0);
  notificationCount$ = this.countSubject.asObservable();

  constructor(
    private api: Api,
    private zone: NgZone,
    private auth: Auth
  ) { }

  async startConnection() {
    if (this.started) return;
    this.started = true;

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${this.api.hubUrl}/notificationHub`, {
        accessTokenFactory: () => this.auth.getToken()
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.on('ReceiveNotification', (data: NotificationDto) => {
      this.zone.run(() => {
        console.log('RECEIVED NOTIFICATION' , data)
        this.notificationSubject.next(data);
        this.newNotificationSubject.next(true);
        this.loadInitialCount();
      });
    });

    await this.hubConnection.start();

    this.loadInitialCount();
  }

  clearNewNotificationFlag() {
    this.newNotificationSubject.next(false);
  }

  public loadInitialCount() {
    this.api.getUnreadNotificationsCount().subscribe(count => {
      this.zone.run(() => {
        this.countSubject.next(count);
      });
    });
  }

}
