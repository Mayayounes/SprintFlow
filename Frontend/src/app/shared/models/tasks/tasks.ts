import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink, RouterOutlet } from '@angular/router';
import { Api } from '../../../core/services/api/api';
import { ErrorModalService } from '../../../core/services/error-modal/error-modal';
import { ToastService } from '../../../core/services/toast/toast';
import { Pagination } from '../../components/pagination/pagination';
import { Auth } from '../../../core/services/Auth/auth';
import { signal } from '@angular/core';
import { UiHelperService } from '../../../core/services/ui-helper/ui-helper';

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

  // tasks: any[] = [];
  tasks = signal<any[]>([]);

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
    rowVersion: ''
  };

  //delete
  showDeleteModal = false;
  deleteTargetId: string | null = null;
  deleteType: 'project' | 'task' | null = null;
  selectedProjectTasksCount = 0;
  selectedProjectName = '';
  constructor(
    private route: ActivatedRoute,
    private api: Api,
    private errorModal: ErrorModalService,
    private toast: ToastService,
    private cdr: ChangeDetectorRef,
    private router: Router,
    private auth: Auth,
    private uiHelper: UiHelperService,
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

        // this.tasks = data?.items ?? [];
        this.tasks.set(data?.items ?? []);
        this.tasksCount = data?.totalItemsCount ?? this.tasks().length;
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
      rowVersion: ''
    };
  }

  openEdit(task: any) {
    task.showMenu = false;

    this.showForm = true;
    this.isEditMode = true;
    this.currentTaskId = task.id;

    this.form = {
      title: task.title,
      description: task.description,
      assignedDate: task.assignedDate,
      deadline: task.deadline,
      rowVersion: task.rowVersion
    };
  }
  submit() {

    const payload = {
      title: this.form.title,
      description: this.form.description,
      assignedDate: this.form.assignedDate || null,
      deadline: this.form.deadline || null,
      rowVersion: this.form.rowVersion
    };

    if (this.isEditMode && this.currentTaskId) {

      this.api.updateTaskDetails(this.projectId, this.currentTaskId, payload)
        .subscribe({
          next: (res: any) => {

            const updated = res.data;

            this.tasks.update(tasks =>
              tasks.map(task =>
                task.id === this.currentTaskId
                  ? {
                    ...task,
                    title: updated.title,
                    description: updated.description,
                    assignedDate: updated.assignedDate,
                    deadline: updated.deadline,
                    rowVersion: updated.rowVersion
                  }
                  : task
              )
            );

            this.toast.show('Task updated successfully', 'success');
            this.showForm = false;
          },
          error: (err) => {

            if (err.error?.message === 'ConcurrencyConflict') {
              this.handleError(err);
              this.loadTasks();
              this.closeForm();
            }

          }
        });

    } else {

      this.api.createTask(this.projectId, payload)
        .subscribe({
          next: () => {
            this.toast.show('Task created successfully', 'success');
            this.loadTasks();
            this.showForm = false;
          },
          error: (err) => {
            this.handleError(err);
            this.showForm = false;
          }
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

        const selectedEmployee = this.employees.find(
          e => e.id === this.employeeId
        );

        this.tasks.update(tasks =>
          tasks.map(task =>
            task.id === this.selectedTaskId
              ? {
                ...task,
                employeeName: selectedEmployee?.userName,
                employeeId: this.employeeId
              }
              : task
          )
        );

        this.toast.show('Employee assigned successfully', 'success');
        this.showAssignModal = false;
      },
      error: (err) => {
        this.handleError(err)
        this.showAssignModal = false;
      }
    });
  }

  trackByTaskId(index: number, task: any) {
    return task.id;
  }

  openAssign(task: any) {
    this.selectedTaskId = task.id;
    this.employeeId = '';
    this.showAssignModal = true;
    this.loadEmployees();
  }

  toggleMenu(task: any) {
    this.tasks().forEach(p => {
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
    this.isEditMode = false;
    this.currentTaskId = null;

    this.form = {
      title: '',
      description: '',
      assignedDate: '',
      deadline: '',
      rowVersion: ''
    };
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

  closeAssignModal() {
    this.showAssignModal = false;
    this.selectedTaskId = null;
    this.employeeId = '';
  }
  formatDuration(seconds: number | null) {
    return this.uiHelper.formatDuration(seconds);
  }

  highlight(text: string) {
    return this.uiHelper.highlight(text, this.SearchTask);
  }
  confirmDeleteTask(task: any) {
    task.showMenu = false;
    this.deleteTargetId = task.id;
    this.deleteType = 'task';
    this.showDeleteModal = true;
  }
  deleteConfirmed() {
    this.api.deleteTask(this.projectId, this.deleteTargetId!).subscribe({
      next: () => {
        this.tasks.update(t => t.filter(x => x.id !== this.deleteTargetId));
        this.toast.show('Task deleted', 'success');
        this.loadTasks();
      },
      error: err => this.handleError(err)
    });
    this.closeDeleteModal();
  }
  closeDeleteModal() {
    this.showDeleteModal = false;
    this.deleteTargetId = null;
    this.deleteType = null;
  }
}
