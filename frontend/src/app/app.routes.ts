import { Routes } from '@angular/router';

// Route tanimlari feature bazli lazy load ediliyor.
// Bu yapi uygulama buyudukce ilk acilis paketini sisirmeden gelismeyi kolaylastirir.
export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./layouts/main-layout/main-layout.component').then((m) => m.MainLayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./features/home/home-page.component').then((m) => m.HomePageComponent),
      },
      {
        path: 'auth',
        loadComponent: () =>
          import('./features/auth/auth-page.component').then((m) => m.AuthPageComponent),
      },
      {
        path: 'jobs',
        loadComponent: () =>
          import('./features/jobs/jobs-page.component').then((m) => m.JobsPageComponent),
      },
      {
        path: 'profile',
        loadComponent: () =>
          import('./features/profile/profile-page.component').then((m) => m.ProfilePageComponent),
      },
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/dashboard/dashboard-page.component').then((m) => m.DashboardPageComponent),
      },
      {
        path: 'ai-tools',
        loadComponent: () =>
          import('./features/ai-tools/ai-tools-page.component').then((m) => m.AiToolsPageComponent),
      },
    ],
  },
  {
    path: '**',
    redirectTo: '',
  },
];
