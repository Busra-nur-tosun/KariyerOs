import { Routes } from '@angular/router';
import { authGuard } from './core/auth/guards/auth.guard';
import { guestGuard } from './core/auth/guards/guest.guard';

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
        data: {
          title: 'KariyerOS | AI destekli kariyer ekosistemi',
          description:
            'KariyerOS ile AI destekli CV optimizasyonu, skill gap analizi, mulakat hazirligi ve kariyer kararlarini tek platformda yonetin.',
          canonical: '/',
        },
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
        canActivate: [authGuard],
        loadComponent: () =>
          import('./features/dashboard/dashboard-page.component').then((m) => m.DashboardPageComponent),
        data: {
          title: 'KariyerOS | Dashboard',
          description: 'KariyerOS hesabinizin korunan dashboard alani.',
          canonical: '/dashboard',
        },
      },
      {
        path: 'ai-tools',
        loadComponent: () =>
          import('./features/ai-tools/ai-tools-page.component').then((m) => m.AiToolsPageComponent),
      },
    ],
  },
  {
    path: '',
    loadComponent: () =>
      import('./layouts/auth-layout/auth-layout.component').then((m) => m.AuthLayoutComponent),
    children: [
      {
        path: 'login',
        canActivate: [guestGuard],
        loadComponent: () =>
          import('./features/auth/login/login-page.component').then((m) => m.LoginPageComponent),
        data: {
          title: 'KariyerOS | Giris Yap',
          description: 'KariyerOS hesabina giris yaparak AI destekli kariyer araclarina erisin.',
          canonical: '/login',
        },
      },
      {
        path: 'register',
        canActivate: [guestGuard],
        loadComponent: () =>
          import('./features/auth/register/register-page.component').then((m) => m.RegisterPageComponent),
        data: {
          title: 'KariyerOS | Kayit Ol',
          description: 'KariyerOS hesabini olustur ve AI destekli kariyer ekosistemine katil.',
          canonical: '/register',
        },
      },
      {
        path: 'auth/google-callback',
        loadComponent: () =>
          import('./features/auth/google-callback/google-callback-page.component').then((m) => m.GoogleCallbackPageComponent),
        data: {
          title: 'KariyerOS | Google Girisi',
          description: 'Google hesabinizla KariyerOS oturumu tamamlanıyor.',
          canonical: '/auth/google-callback',
        },
      },
    ],
  },
  {
    path: 'auth',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: '**',
    redirectTo: '',
  },
];
