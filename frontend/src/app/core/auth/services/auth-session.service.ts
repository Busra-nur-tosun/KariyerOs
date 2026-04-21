import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';

import {
  AuthResponse,
  AuthSession,
  CurrentUserResponse,
  LoginRequest,
  RegisterRequest,
} from '../models/auth.models';
import { AuthApiService } from './auth-api.service';
import { TokenStorageService } from './token-storage.service';

@Injectable({ providedIn: 'root' })
export class AuthSessionService {
  private readonly authApi = inject(AuthApiService);
  private readonly tokenStorage = inject(TokenStorageService);
  private readonly router = inject(Router);

  private readonly currentUserState = signal<CurrentUserResponse | null>(null);
  private readonly initializingState = signal(false);

  readonly currentUser = computed(() => this.currentUserState());
  readonly isAuthenticated = computed(() => this.currentUserState() !== null && this.hasValidAccessToken());
  readonly isInitializing = computed(() => this.initializingState());

  async initialize(): Promise<void> {
    const session = this.tokenStorage.getSession();

    if (!session) {
      this.currentUserState.set(null);
      return;
    }

    this.initializingState.set(true);

    try {
      if (!this.isFutureDate(session.accessTokenExpiresAtUtc) && this.isFutureDate(session.refreshTokenExpiresAtUtc)) {
        const refreshed = await firstValueFrom(
          this.authApi.refresh({
            refreshToken: session.refreshToken,
          }),
        );

        this.persistSession(refreshed.data);
      }

      const currentUser = await firstValueFrom(this.authApi.me());
      this.currentUserState.set(currentUser.data);
    } catch {
      this.clearSession();
    } finally {
      this.initializingState.set(false);
    }
  }

  async login(payload: LoginRequest): Promise<CurrentUserResponse> {
    const response = await firstValueFrom(this.authApi.login(payload));
    return this.applyAuthentication(response.data);
  }

  async register(payload: RegisterRequest): Promise<CurrentUserResponse> {
    const response = await firstValueFrom(this.authApi.register(payload));
    return this.applyAuthentication(response.data);
  }

  async refreshSession(): Promise<string | null> {
    const session = this.tokenStorage.getSession();

    if (!session || !this.isFutureDate(session.refreshTokenExpiresAtUtc)) {
      this.clearSession();
      return null;
    }

    try {
      const response = await firstValueFrom(
        this.authApi.refresh({
          refreshToken: session.refreshToken,
        }),
      );

      this.persistSession(response.data);

      if (!this.currentUserState()) {
        const currentUser = await firstValueFrom(this.authApi.me());
        this.currentUserState.set(currentUser.data);
      }

      return response.data.accessToken;
    } catch {
      this.clearSession();
      return null;
    }
  }

  logout(redirectTo = '/'): void {
    this.clearSession();
    void this.router.navigateByUrl(redirectTo);
  }

  completeExternalAuthentication(session: AuthSession): void {
    this.tokenStorage.setSession(session);
  }

  async loadCurrentUser(): Promise<CurrentUserResponse> {
    const currentUser = await firstValueFrom(this.authApi.me());
    this.currentUserState.set(currentUser.data);
    return currentUser.data;
  }

  getAccessToken(): string | null {
    const session = this.tokenStorage.getSession();

    if (!session || !this.isFutureDate(session.accessTokenExpiresAtUtc)) {
      return null;
    }

    return session.accessToken;
  }

  private async applyAuthentication(response: AuthResponse): Promise<CurrentUserResponse> {
    this.persistSession(response);
    return this.loadCurrentUser();
  }

  private persistSession(response: AuthResponse): void {
    const session: AuthSession = {
      accessToken: response.accessToken,
      accessTokenExpiresAtUtc: response.accessTokenExpiresAtUtc,
      refreshToken: response.refreshToken,
      refreshTokenExpiresAtUtc: response.refreshTokenExpiresAtUtc,
    };

    this.tokenStorage.setSession(session);
  }

  private clearSession(): void {
    this.tokenStorage.clear();
    this.currentUserState.set(null);
  }

  private hasValidAccessToken(): boolean {
    const session = this.tokenStorage.getSession();
    return session !== null && this.isFutureDate(session.accessTokenExpiresAtUtc);
  }

  private isFutureDate(value: string): boolean {
    return new Date(value).getTime() > Date.now();
  }
}
