import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NotificationDto } from '../notification/notification.model';

@Injectable({
  providedIn: 'root',
})
export class Api {
  private baseUrl = 'http://localhost:5134/api';
  public hubUrl = 'http://localhost:5134';

  constructor(private http: HttpClient) { }
  //Auth
  AddUser(data: any) {
    return this.http.post(`${this.baseUrl}/addUser`, data);
  }
  login(data: any) {
    return this.http.post(`${this.baseUrl}/login`, data);
  }
  //Dashboard
  GetStats() {
    return this.http.get(`${this.baseUrl}/dashboard/stats`);
  }
  //Identity
  updateUser(userId: string, data: any) {
    return this.http.put(`${this.baseUrl}/identity/edit/${userId}`, data);
  }
  deleteUser(userId: string) {
    return this.http.delete(`${this.baseUrl}/identity/deleteUser/${userId}`);
  }
  //My tasks
  getMyTasks(pageNumber: number, pageSize: number, status?: string) {
    let params = new HttpParams()
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (status) {
      params = params.set('status', status);
    }

    return this.http.get(`${this.baseUrl}/tasks/my-tasks`, { params });
  }
  //projects
  getProjects(searchPhrase: string, pageNumber: number, pageSize: number) {
    let params = new HttpParams()
      .set('searchPhrase', searchPhrase)
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);

    if (searchPhrase && searchPhrase.trim() !== '') {
      params = params.set('searchPhrase', searchPhrase);
    }
    return this.http.get(`${this.baseUrl}/projects`, { params });
  }
  createProject(data: any) {
    return this.http.post(`${this.baseUrl}/projects/create`, data);
  }
  editProject(projectId: string, data: any) {
    return this.http.patch(`${this.baseUrl}/projects/${projectId}`, data);
  }

  //Users
  getAllUsers(searchRole: string, pageNumber: number, pageSize: number) {
    const params = new HttpParams()
      .set('searchRole', searchRole)
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);
    return this.http.get(`${this.baseUrl}/users`, { params });
  }

  //tasks
  createTask(projectId: string, data: any) {
    return this.http.post(`${this.baseUrl}/projects/${projectId}/tasks/create`, data);
  }
  getAllTasksForProject(projectId: string, SearchTask: string, pageNumber: number, pageSize: number) {
    const params = new HttpParams()
      .set('SearchTask', SearchTask)
      .set('pageNumber', pageNumber)
      .set('pageSize', pageSize);
    return this.http.get(`${this.baseUrl}/projects/${projectId}/tasks`, { params });
  }
  getTaskForProject(projectId: string, taskId: string) {
    return this.http.get(`${this.baseUrl}/projects/${projectId}/tasks/${taskId}`);
  }
  updateTaskStatus(projectId: string, taskId: string, data: any) {
    return this.http.patch(`${this.baseUrl}/projects/${projectId}/tasks/${taskId}/updateStatus`, data);
  }
  updateTaskDetails(projectId: string, taskId: string, data: any) {
    return this.http.patch(`${this.baseUrl}/projects/${projectId}/tasks/${taskId}/update`, data);
  }
  assignEmployeeToTask(projectId: string, taskId: string, data: any) {
    return this.http.post(`${this.baseUrl}/projects/${projectId}/tasks/${taskId}/assignEmployee`, data)
  }
  getTasksByStatus(projectId: string, status: number) {
    const params = new HttpParams().set('status', status);
    return this.http.get(`${this.baseUrl}/projects/${projectId}/tasks/filter`, { params });
  }
  //get all roles
  getRoles() {
    return this.http.get<string[]>(`${this.baseUrl}/users/roles`);
  }
  //Notification
  getNotifications() {
    return this.http.get<NotificationDto[]>(
      `${this.baseUrl}/notifications`
    );
  }
  getUnreadNotificationsCount() {
    return this.http.get<number>(
      `${this.baseUrl}/notifications/unread-count`
    );
  }
  markNotificationRead(notificationId: string) {
    return this.http.put(
      `${this.baseUrl}/notifications/${notificationId}/read`,
      {}
    );
  }
  markAllNotificationsRead() {
    return this.http.put(
      `${this.baseUrl}/notifications/mark-all-read`,
      {}
    );
  }
}
