import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Api } from '../../../core/services/api/api';
import { ErrorModalService } from '../../../core/services/error-modal/error-modal';
import { ToastService } from '../../../core/services/toast/toast';
import { Pagination } from '../../components/pagination/pagination';
import { Auth } from '../../../core/services/Auth/auth';
import { signal, effect } from '@angular/core';

@Component({
  selector: 'app-projects',
  imports: [CommonModule, FormsModule, Pagination],
  standalone: true,
  templateUrl: './projects.html',
  styleUrl: './projects.css',
})
export class Projects implements OnInit {

  isAdminRoute = false;

  projects: any[] = [];

  // searchPhrase: string = '';
  searchTask = signal('');
  pageNumber = 1;
  totalPages = 1;
  pageSize = 5;
  pageSizes = [5, 10, 15, 30];

  showForm = false;
  isEditMode = false;
  loading = false;
  currentProjectId: string | null = null;

  projectsCount = 0;

  form = {
    name: '',
    description: ''
  };

  constructor(
    private api: Api,
    private router: Router,
    private errorModal: ErrorModalService,
    private toast: ToastService,
    private cdr: ChangeDetectorRef,
    private auth: Auth
  ) {
    effect(() => {
      this.searchTask();
      this.pageNumber = 1;
      this.loadProjects();
    });
  }

  ngOnInit() {
    console.log('ROLE FROM SERVICE:', this.auth.getRole());
    this.loadProjects();
  }

  get isAdmin(): boolean {
    return this.auth.isAdmin();
  }

  loadProjects() {
    this.loading = true;

    this.api.getProjects(this.searchTask(), this.pageNumber, this.pageSize)
      .subscribe({
        next: (res: any) => {
          const data = res?.data;

          this.projects = data?.items ?? [];
          this.projectsCount = data?.totalItemsCount ?? this.projects.length;
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

  // onSearchChange() {
  //   this.pageNumber = 1;
  //   this.loadProjects();
  // }

  highlight(text: string): string {
    const search = this.searchTask();

    if (!search) return text;

    const escaped = search.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
    const regex = new RegExp(`(${escaped})`, 'gi');

    return text.replace(
      regex,
      `<mark class="bg-yellow-200 text-white px-1 rounded">$1</mark>`
    );
  }

  openCreateForm() {
    this.showForm = true;
    this.isEditMode = false;
    this.currentProjectId = null;

    this.form = { name: '', description: '' };
  }

  openEditForm(project: any) {
    this.showForm = true;
    this.isEditMode = true;
    this.currentProjectId = project.id;

    this.form = {
      name: project.name,
      description: project.description
    };
  }

  closeForm() {
    this.showForm = false;
  }

  submitForm() {
    if (this.isEditMode && this.currentProjectId) {

      this.api.editProject(this.currentProjectId, this.form)
        .subscribe({
          next: () => {
            this.toast.show('Project updated successfully', 'success');
            this.loadProjects();
            this.closeForm();
          },
          error: (err) => this.handleError(err)
        });

    } else {

      this.api.createProject(this.form)
        .subscribe({
          next: () => {
            this.toast.show('Project created successfully', 'success');
            this.loadProjects();
            this.closeForm();
          },
          error: (err) => this.handleError(err)
        });

    }
  }

  goToTasks(project: any) {
    this.router.navigate([`/${this.auth.getRole()}/projects`, project.id, 'tasks'], { state: { projectName: project.name } });
  }

  toggleMenu(project: any) {
    this.projects.forEach(p => {
      if (p !== project) p.showMenu = false;
    });
    project.showMenu = !project.showMenu;
  }

  onPageChange(page: number) {
    this.pageNumber = page;
    this.loadProjects();
  }

  onPageSizeChange(size: number) {
    this.pageSize = size;
    this.pageNumber = 1;
    this.loadProjects();
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
