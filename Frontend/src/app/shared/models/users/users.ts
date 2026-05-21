import { ChangeDetectorRef, Component, HostListener, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Api } from '../../../core/services/api/api';
import { ErrorModalService } from '../../../core/services/error-modal/error-modal';
import { Pagination } from '../../components/pagination/pagination';
import { ToastService } from '../../../core/services/toast/toast';
import { signal } from '@angular/core';
@Component({
  selector: 'app-user',
  standalone: true,
  imports: [CommonModule, FormsModule, Pagination],
  templateUrl: './users.html',
  styleUrl: './users.css',
})
export class Users implements OnInit {

  // users: any[] = [];
  users = signal<any[]>([]);

  searchRole = '';
  pageNumber = 1;
  pageSize = 5;
  totalPages = 1;
  pageSizes = [5, 10, 15, 30];

  loading = false;

  usersCount = 0;
  employeesCount = 0;
  leadersCount = 0;

  showForm = false;
  isEditMode = false;
  currentUserId: string | null = null;
  roles: string[] = [];
  form = {
    userName: '',
    email: '',
    password: '',
    phoneNumber: '',
    role: ''
  };

  constructor(private api: Api, private cdr: ChangeDetectorRef, private errorModal: ErrorModalService, private toast: ToastService) { }

  ngOnInit() {
    this.loadUsers();
    this.loadRoles();
  }

  @HostListener('document:click', ['$event'])

  onClickOutside(event: MouseEvent) {
    const target = event.target as HTMLElement;
    if (target.closest('button')) return;
    this.users().forEach(u => u.showMenu = false);
  }
  loadUsers() {
    this.loading = true;

    this.api.getAllUsers(this.searchRole, this.pageNumber, this.pageSize)
      .subscribe({
        next: (res: any) => {
          const data = res?.data;
          // this.users = data?.items ?? [];
          this.users.set(data?.items ?? []);
          this.usersCount = data?.totalItemsCount ?? this.users().length;
          this.employeesCount = this.users().filter((u: any) => u.role === 'Employee').length;
          this.leadersCount = this.users().filter((u: any) => u.role === 'Leader').length;
          this.totalPages = data?.totalPages ?? 1;
          this.loading = false;
          this.cdr.detectChanges();
        },

        error: (err) => {
          const backendErrors = err?.error?.errors;

          if (backendErrors) {
            this.errorModal.show(backendErrors);
          } else {
            this.errorModal.show('Unexpected error occurred');
          }

          this.loading = false;
          this.cdr.detectChanges();
        }
      });
  }

  deleteUser(user: any) {
    user.showMenu = false;
    this.api.deleteUser(user.id).subscribe({

      next: () => {
        this.toast.show('User deleted successfully', 'success');
        this.loadUsers();
      },

      error: (err) => {
        const backendErrors = err?.error?.errors;

        if (backendErrors) {
          this.errorModal.show(backendErrors);
        } else {
          this.errorModal.show('Unexpected error occurred');
        }

        this.loading = false;
        this.cdr.detectChanges();
      }

    });
  }

  onRoleChange() {
    this.pageNumber = 1;
    this.loadUsers();
  }

  onPageChange(page: number) {
    this.pageNumber = page;
    this.loadUsers();
  }

  onPageSizeChange(size: number) {
    this.pageSize = size;
    this.pageNumber = 1;
    this.loadUsers();
  }

  openCreateForm() {
    if (!this.roles.length) {
      this.loadRoles();
    }

    this.showForm = true;

    this.isEditMode = false;
    this.currentUserId = null;

    this.resetForm();
  }

  closeForm() {
    this.showForm = false;
  }

  resetForm() {
    this.form = {
      userName: '',
      email: '',
      password: '',
      phoneNumber: '',
      role: ''
    };
  }

  loadRoles() {
    this.api.getRoles().subscribe({
      next: (res: any) => {
        this.roles = res ?? [];
      },
      error: () => {
        this.roles = [];
      }
    });
  }

  submitForm() {

    if (this.isEditMode && this.currentUserId) {

      const payload = {
        email: this.form.email,
        userName: this.form.userName,
        phoneNumber: this.form.phoneNumber
      };

      this.api.updateUser(this.currentUserId, payload)
        .subscribe({
          next: () => {

            this.users.update(users =>
              users.map(user =>
                user.id === this.currentUserId
                  ? {
                    ...user,
                    userName: this.form.userName,
                    phoneNumber: this.form.phoneNumber
                  }
                  : user
              )
            );

            this.toast.show('User updated successfully', 'success');
            this.closeForm();
          },
          error: (err) => this.handleError(err)
        });

    } else {

      if (!this.form.email || !this.form.password || !this.form.role) {
        this.errorModal.show("All fields are required");
        return;
      }

      this.api.AddUser(this.form)
        .subscribe({
          next: () => {
            this.toast.show('User Added Successfully', 'success');
            this.closeForm();
            this.loadUsers();
          },
          error: (err) => this.handleError(err)
        });
    }
  }

  toggleMenu(user: any) {
    this.users().forEach(u => {
      if (u !== user) u.showMenu = false;
    });
    user.showMenu = !user.showMenu;
  }

  trackByUserId(index: number, user: any) {
    return user.id;
  }
  openEditForm(user: any) {
    this.showForm = true;
    this.isEditMode = true;
    this.currentUserId = user.id;
    console.log('current_user', this.currentUserId);
    this.form = {
      userName: user.userName,
      email: user.email,
      password: '',
      phoneNumber: user.phoneNumber,
      role: user.role
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
}
