import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Api } from '../../../core/services/api/api';
import { Auth } from '../../../core/services/Auth/auth';

interface EmployeePerformance {
  employeeId: string;
  employeeName: string;
  employeeEmail: string;
  assignedTasks: number;
  completedTasks: number;
  earlySubmissions: number;
  onTimeSubmissions: number;
  lateSubmissions: number;
  completionRate: number;
  onTimeRate: number;
  score: number;
}

interface DashboardStats {
  users: number;
  admins: number;
  leaders: number;
  employees: number;
  projects: number;
  doneProjects: number;
  pendingProjects: number;
  employeeOfMonthLabel: string;
  employeeAnalyticsMonthLabel: string;
  assignedTasksThisMonth: number;
  completedTasksThisMonth: number;
  earlyTasksThisMonth: number;
  onTimeTasksThisMonth: number;
  lateTasksThisMonth: number;
  employeeOfMonth: EmployeePerformance | null;
  topEmployees: EmployeePerformance[];
}

@Component({
  selector: 'app-statistics',
  imports: [CommonModule],
  standalone: true,
  templateUrl: './statistics.html',
  styleUrl: './statistics.css',
})
export class Statistics {
  role: string = '';

  stats: DashboardStats = {
    users: 0,
    admins: 0,
    leaders: 0,
    employees: 0,
    projects: 0,
    doneProjects: 0,
    pendingProjects: 0,
    employeeOfMonthLabel: 'Employee of the Month',
    employeeAnalyticsMonthLabel: '',
    assignedTasksThisMonth: 0,
    completedTasksThisMonth: 0,
    earlyTasksThisMonth: 0,
    onTimeTasksThisMonth: 0,
    lateTasksThisMonth: 0,
    employeeOfMonth: null,
    topEmployees: []
  };

  loading = false;

  constructor(
    private api: Api,
    private auth: Auth,
    private cdr : ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.role = this.auth.getRole().toLowerCase();
    this.loadStats();
  }

  loadStats() {
    this.loading = true;

    this.api.GetStats().subscribe({
      next: (res: any) => {
        this.stats = res;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Stats error:', err);
        this.loading = false;
      }
    });
  }

  get topEmployeeMaxScore() {
    return Math.max(...this.stats.topEmployees.map(employee => employee.score), 1);
  }

  scoreWidth(score: number) {
    return `${Math.max((score / this.topEmployeeMaxScore) * 100, 8)}%`;
  }

  displayEmployeeName(employee: EmployeePerformance | null) {
    return employee?.employeeName || employee?.employeeEmail || 'No employee yet';
  }
}
