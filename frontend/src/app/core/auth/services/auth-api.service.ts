import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { environment } from '../../../../environments/environment';
import {
  ApiResponse,
  AuthResponse,
  CurrentUserResponse,
  LoginRequest,
  RefreshTokenRequest,
  RegisterRequest,
} from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class AuthApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiBaseUrl;

  register(payload: RegisterRequest) {
    return this.http.post<ApiResponse<AuthResponse>>(`${this.baseUrl}/auth/register`, payload);
  }

  login(payload: LoginRequest) {
    return this.http.post<ApiResponse<AuthResponse>>(`${this.baseUrl}/auth/login`, payload);
  }

  refresh(payload: RefreshTokenRequest) {
    return this.http.post<ApiResponse<AuthResponse>>(`${this.baseUrl}/auth/refresh`, payload);
  }

  me() {
    return this.http.get<ApiResponse<CurrentUserResponse>>(`${this.baseUrl}/users/me`);
  }
}
