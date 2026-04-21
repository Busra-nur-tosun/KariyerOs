import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, from, switchMap, throwError } from 'rxjs';

import { environment } from '../../../../environments/environment';
import { AuthSessionService } from '../services/auth-session.service';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  const authSession = inject(AuthSessionService);
  const router = inject(Router);
  const isApiRequest = request.url.startsWith(environment.apiBaseUrl);
  const token = authSession.getAccessToken();

  const authenticatedRequest =
    isApiRequest && token
      ? request.clone({
          setHeaders: {
            Authorization: `Bearer ${token}`,
          },
        })
      : request;

  return next(authenticatedRequest).pipe(
    catchError((error: unknown) => {
      const httpError = error as HttpErrorResponse;
      const isRefreshRequest = request.url.endsWith('/auth/refresh');
      const isPublicAuthRequest = request.url.endsWith('/auth/login') || request.url.endsWith('/auth/register');

      if (!isApiRequest || httpError.status !== 401 || isRefreshRequest || isPublicAuthRequest) {
        return throwError(() => error);
      }

      return from(authSession.refreshSession()).pipe(
        switchMap((newToken) => {
          if (!newToken) {
            void router.navigateByUrl('/login');
            return throwError(() => error);
          }

          return next(
            request.clone({
              setHeaders: {
                Authorization: `Bearer ${newToken}`,
              },
            }),
          );
        }),
        catchError((refreshError) => {
          authSession.logout('/login');
          return throwError(() => refreshError);
        }),
      );
    }),
  );
};
