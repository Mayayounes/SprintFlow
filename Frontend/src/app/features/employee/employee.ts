import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Api } from '../../core/services/api/api';
import { ToastService } from '../../core/services/toast/toast';
import { ErrorModalService } from '../../core/services/error-modal/error-modal';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Pagination } from '../../shared/components/pagination/pagination';

@Component({
  selector: 'app-employee',
  imports: [CommonModule, FormsModule, Pagination],
  templateUrl: './employee.html',
  styleUrl: './employee.css',
})
export class Employee implements OnInit {

  tasks: any[] = [];
  loading = false;

  pageNumber = 1;
  pageSize = 5;
  totalPages = 1;
  pageSizes = [5, 10, 15, 30];

  statusFilter: string = '';

  statusOptions = [
    { label: 'All', value: '' },
    { label: 'ToDo', value: 'ToDo' },
    { label: 'InProgress', value: 'InProgress' },
    { label: 'Done', value: 'Done' },
  ];

  statusMap: any = {
    ToDo: 0,
    InProgress: 1,
    Done: 2
  };
  showStatusModal = false;
  selectedTask: any = null;

  constructor(
    private api: Api,
    private toast: ToastService,
    private errorModal: ErrorModalService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit() {
    this.loadMyTasks();
  }

  loadMyTasks() {
    this.loading = true;

    this.api.getMyTasks(this.pageNumber, this.pageSize, this.statusFilter)
      .subscribe({
        next: (res: any) => {
          const data = res.data;

          this.tasks = data.items ?? [];
          this.totalPages = data.totalPages ?? 1;

          this.loading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          this.loading = false;
          this.handleError(err);
        }
      });
  }

  // FILTER STATUS
  onStatusChange() {
    this.pageNumber = 1;
    this.loadMyTasks();
  }

  // PAGINATION
  onPageChange(page: number) {
    this.pageNumber = page;
    this.loadMyTasks();
  }

  onPageSizeChange(size: number) {
    this.pageSize = size;
    this.pageNumber = 1;
    this.loadMyTasks();
  }

  // STATUS MODAL
  openStatusModal(task: any) {
    this.selectedTask = { ...task };
    this.showStatusModal = true;
  }

  updateStatus() {
    if (!this.selectedTask) return;

    const payload = {
      status: this.statusMap[this.selectedTask.status]
    };

    this.api.updateTaskStatus(
      this.selectedTask.projectId,
      this.selectedTask.id,
      payload
    ).subscribe({
      next: () => {
        this.toast.show('Status updated successfully', 'success');

        const task = this.tasks.find(t => t.id === this.selectedTask.id);

        if (task) {
          task.status = this.selectedTask.status;
        }

        this.showStatusModal = false;
        this.selectedTask = null;

        this.cdr.detectChanges();
      },
      error: (err) => this.handleError(err)
    });
  }

  getStatusColor(status: string) {
    if (status === 'ToDo') return 'bg-red-500';
    if (status === 'InProgress') return 'bg-yellow-500';
    return 'bg-green-500';
  }

  private handleError(err: any) {
    const errors = err?.error?.errors;
    this.errorModal.show(errors || 'Unexpected error occurred');
    this.cdr.detectChanges();
  }
}
