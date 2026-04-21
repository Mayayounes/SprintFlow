import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Api } from '../../../core/services/api/api';
import { Auth } from '../../../core/services/Auth/auth';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-statistics',
  imports: [CommonModule],
  standalone: true,
  templateUrl: './statistics.html',
  styleUrl: './statistics.css',
})
export class Statistics {
  role: string = '';

  stats = {
    users: 0,
    admins: 0,
    leaders: 0,
    employees: 0,
    projects: 0
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
}
