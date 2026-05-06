import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink, RouterOutlet } from '@angular/router';
import { Api } from '../../../core/services/api/api';
import { ErrorModalService } from '../../../core/services/error-modal/error-modal';
import { ToastService } from '../../../core/services/toast/toast';
import { Pagination } from '../../components/pagination/pagination';
import { Auth } from '../../../core/services/Auth/auth';

@Component({
  selector: 'app-tasks',
  imports: [CommonModule, FormsModule, Pagination],
  standalone: true,
  templateUrl: './tasks.html',
  styleUrl: './tasks.css',
})
export class Tasks implements OnInit {
  projectId!: string;
  projectName: string = '';

  tasks: any[] = [];
  employees: any[] = [];

  selectedTaskId: string | null = null;
  showAssignModal = false;
  employeeId: string = '';

  loading = false;

  pageNumber = 1;
  totalPages = 1;
  pageSize = 5;
  tasksCount = 0;
  SearchTask: string = '';
  pageSizes = [5, 10, 15, 30];

  showForm = false;
  isEditMode = false;
  currentTaskId: string | null = null;

  form = {
    title: '',
    description: '',
    assignedDate: '',
    deadline: '',
  };

  constructor(
    private route: ActivatedRoute,
    private api: Api,
    private errorModal: ErrorModalService,
    private toast: ToastService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private auth: Auth
  ) { }

  ngOnInit() {
    console.log('ROLE FROM SERVICE:', this.auth.getRole());
    this.projectId = this.route.snapshot.params['projectId'];
    this.projectName = history.state.projectName;
    this.loadTasks();
  }

  get isAdmin(): boolean {
    return this.auth.isAdmin();
  }

  loadTasks() {
    this.loading = true;

    this.api.getAllTasksForProject(this.projectId, this.SearchTask, this.pageNumber, this.pageSize).subscribe({
      next: (res: any) => {
        const data = res?.data;

        this.tasks = data?.items ?? [];
        this.tasksCount = data?.totalItemsCount ?? this.tasks.length;
        this.totalPages = data?.totalPages ?? 1;

        this.loading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.loading = false;
        this.handleError(err);
      }
    });
  }

  loadEmployees() {
    this.api.getAllUsers('Employee', 1, 30).subscribe({
      next: (res: any) => {
        this.employees = res?.data?.items ?? [];
        this.cdr.detectChanges();
      },
      error: (err) => this.handleError(err)
    });
  }

  openCreate() {
    this.showForm = true;
    this.isEditMode = false;
    this.currentTaskId = null;

    this.form = {
      title: '',
      description: '',
      assignedDate: '',
      deadline: '',
    };
  }

  openEdit(task: any) {
    this.showForm = true;
    this.isEditMode = true;
    this.currentTaskId = task.id;

    this.form = {
      title: task.title,
      description: task.description,
      assignedDate: task.assignedDate,
      deadline: task.deadline,
    };
  }

  submit() {

    const payload = {
      title: this.form.title,
      description: this.form.description,
      assignedDate: this.form.assignedDate || null,
      deadline: this.form.deadline || null
    };

    if (this.isEditMode && this.currentTaskId) {

      this.api.updateTaskDetails(this.projectId, this.currentTaskId, payload)
        .subscribe({
          next: () => {
            this.toast.show('Task updated successfully', 'success');
            this.loadTasks();
            this.showForm = false;
          },
          error: (err) => this.handleError(err)
        });

    } else {

      this.api.createTask(this.projectId, payload)
        .subscribe({
          next: () => {
            this.toast.show('Task created successfully', 'success');
            this.loadTasks();
            this.showForm = false;
          },
          error: (err) => this.handleError(err)
        });

    }
  }

  assignEmployee() {
    if (!this.employeeId || !this.selectedTaskId) {
      this.errorModal.show("Please select an employee");
      return;
    }

    this.api.assignEmployeeToTask(
      this.projectId,
      this.selectedTaskId,
      { employeeId: this.employeeId }
    ).subscribe({
      next: () => {
        this.toast.show('Employee assigned successfully', 'success');
        this.showAssignModal = false;
        this.loadTasks();
      },
      error: (err) => this.handleError(err)
    });
  }

  openAssign(task: any) {
    this.selectedTaskId = task.id;
    this.employeeId = '';
    this.showAssignModal = true;
    this.loadEmployees();
  }

  toggleMenu(task: any) {
    this.tasks.forEach(p => {
      if (p !== task) p.showMenu = false;
    });
    task.showMenu = !task.showMenu;
  }

  getStatusColor(status: any) {

  switch (status) {

    case 'toDo':
    case 0:
      return 'bg-rose-500/15 text-rose-300 border border-rose-400/30';

    case 'inProgress':
    case 1:
      return 'bg-amber-500/15 text-amber-300 border border-amber-400/30';

    case 'done':
    case 2:
      return 'bg-emerald-500/15 text-emerald-300 border border-emerald-400/30';

    default:
      return 'bg-slate-500/15 text-slate-300 border border-slate-400/20';
  }
}
  goBack() {
    const role = this.auth.getRole()?.toLowerCase();
    this.router.navigate([`/${role}/projects`]);
  }

  closeForm() {
    this.showForm = false;
  }

  private handleError(err: any) {
    const backendErrors = err?.error?.errors;

    if (backendErrors) {
      this.errorModal.show(backendErrors);
    } else {
      this.errorModal.show('Unexpected error occurred');
    }

    this.cdr.detectChanges();
  }

formatDuration(duration: string): string {
  if (!duration) return '';

  const parts = duration.split(':').map(Number);
  let hours = parts[0];
  const minutes = parts[1];
  const seconds = parts[2];

  let days = Math.floor(hours / 24);
  hours = hours % 24;

  let weeks = Math.floor(days / 7);
  days = days % 7;

  const result: string[] = [];

  if (weeks > 0) result.push(`${weeks} week${weeks > 1 ? 's' : ''}`);
  if (days > 0) result.push(`${days} day${days > 1 ? 's' : ''}`);
  if (hours > 0) result.push(`${hours} hour${hours > 1 ? 's' : ''}`);
  if (minutes > 0) result.push(`${minutes} minute${minutes > 1 ? 's' : ''}`);
  if (seconds > 0 && result.length === 0) {
    result.push(`${seconds} second${seconds > 1 ? 's' : ''}`);
  }

  return result.join(' ');
}

  onPageChange(page: number) {
    this.pageNumber = page;
    this.loadTasks();
  }

  onPageSizeChange(size: number) {
    this.pageSize = size;
    this.pageNumber = 1;
    this.loadTasks();
  }

  onSearchChange() {
    this.pageNumber = 1;
    this.loadTasks();
  }

  highlight(text: string): string {
    if (!this.SearchTask) return text;

    const regex = new RegExp(`(${this.SearchTask})`, 'gi');
    return text.replace(
      regex,
      `<mark class="bg-yellow-200 text-white px-1 rounded">$1</mark>`
    );
  }
}
