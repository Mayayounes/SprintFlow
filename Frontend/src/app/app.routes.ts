import { Routes } from '@angular/router';
import { Login } from './features/auth/login';
import { Dashboard } from './features/layout/dashboard/dashboard';
import { Admin } from './features/admin/admin';
import { Statistics } from './features/layout/statistics/statistics';
import { Leader } from './features/leader/leader';
import { Employee } from './features/employee/employee';
import { MainComponent } from './features/layout/main-component/main-component';
import { Users } from './shared/models/users/users';
import { Projects } from './shared/models/projects/projects';
import { Tasks } from './shared/models/tasks/tasks';
import { RoleGuard } from './core/guards/RoleGuard';
import { AuthGuard } from './core/guards/AuthGuard';
import { GuestGuard } from './core/guards/GuestGuard';
import { NotFound } from './shared/components/not-found/not-found';

export const routes: Routes = [

  { path: '', component: Dashboard },
  {
    path: 'auth',
    component: Login,
    canActivate: [GuestGuard]
  },
  {
    path: '',
    component: MainComponent,
    canActivate: [AuthGuard],
    children: [
      {
        path: 'admin',
        component: Admin,
        canActivate: [RoleGuard],
        data: { role: 'admin' },
        children: [
          { path: '', redirectTo: 'statistics', pathMatch: 'full' },
          { path: 'statistics', component: Statistics },
          { path: 'users', component: Users },
          { path: 'projects', component: Projects },
          { path: 'projects/:projectId/tasks', component: Tasks },
        ]
      },
      {
        path: 'leader',
        component: Leader,
        canActivate: [RoleGuard],
        data: { role: 'leader' },
        children: [
          { path: '', redirectTo: 'statistics', pathMatch: 'full' },
          { path: 'statistics', component: Statistics },
          { path: 'projects', component: Projects },
          { path: 'projects/:projectId/tasks', component: Tasks },
        ]
      },
      {
        path: 'employee',
        component: Employee,
        canActivate: [RoleGuard],
        data: { role: 'employee' },
        children: [
          { path: 'projects', component: Projects },
        ]
      }
    ]
  },
  { path: '**', component: NotFound }
];
