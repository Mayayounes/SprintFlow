export interface NotificationDto {
  id: string;
  message: string;
  isRead: boolean;
  createdAt: string;
  updatedAt : string;
  unreadCount: number;
}
