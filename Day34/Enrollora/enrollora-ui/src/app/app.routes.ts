import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth-guard';
import { roleGuard } from './core/guards/role-guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then((m) => m.Login),
  },
  {
    path: 'register',
    loadComponent: () => import('./features/auth/register/register').then((m) => m.Register),
  },
  {
    path: 'admin',
    canActivate: [authGuard, roleGuard],
    data: { role: 'Administrator' },
    loadComponent: () => import('./layouts/admin-layout/admin-layout').then((m) => m.AdminLayout),
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/admin/dashboard/dashboard').then((m) => m.Dashboard),
      },
      {
        path: 'students',
        loadComponent: () =>
          import('./features/admin/students/student-list/student-list').then((m) => m.StudentList),
      },
      {
        path: 'courses',
        loadComponent: () =>
          import('./features/admin/courses/course-list/course-list').then(
            (m) => m.CourseListComponent,
          ),
      },

      {
        path: 'enrollments',
        loadComponent: () =>
          import('./features/enrollments/admin/enrollment-list/enrollment-list').then(
            (m) => m.EnrollmentListComponent,
          ),
      },
    ],
  },
  {
    path: 'student',
    canActivate: [authGuard, roleGuard],
    data: { role: 'Student' },
    loadComponent: () =>
      import('./layouts/student-layout/student-layout').then((m) => m.StudentLayout),
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/student/dashboard/dashboard').then((m) => m.Dashboard),
      },
      {
        path: 'courses',
        loadComponent: () =>
          import('./features/enrollments/student/available-courses/available-courses').then(
            (m) => m.AvailableCourses,
          ),
      },
      {
        path: 'enrollments',
        loadComponent: () =>
          import('./features/enrollments/student/my-enrollments/my-enrollments').then(
            (m) => m.MyEnrollments,
          ),
      },
      {
        path: '',
        redirectTo: 'courses',
        pathMatch: 'full',
      },
    ],
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: 'login',
  },
];
