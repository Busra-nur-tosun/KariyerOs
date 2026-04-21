import { Component, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';

import { AuthSession } from '../../../core/auth/models/auth.models';
import { AuthSessionService } from '../../../core/auth/services/auth-session.service';

@Component({
  selector: 'app-google-callback-page',
  imports: [RouterLink],
  templateUrl: './google-callback-page.component.html',
})
export class GoogleCallbackPageComponent {
  private readonly authSession = inject(AuthSessionService);
  private readonly router = inject(Router);

  protected readonly errorMessage = signal<string | null>(null);

  constructor() {
    void this.completeAuthentication();
  }

  private async completeAuthentication(): Promise<void> {
    const hash = new URLSearchParams(window.location.hash.replace(/^#/, ''));
    const error = hash.get('error');

    if (error) {
      this.errorMessage.set(error);
      return;
    }

    const accessToken = hash.get('accessToken');
    const accessTokenExpiresAtUtc = hash.get('accessTokenExpiresAtUtc');
    const refreshToken = hash.get('refreshToken');
    const refreshTokenExpiresAtUtc = hash.get('refreshTokenExpiresAtUtc');

    if (!accessToken || !accessTokenExpiresAtUtc || !refreshToken || !refreshTokenExpiresAtUtc) {
      this.errorMessage.set('Google girisi tamamlanamadi.');
      return;
    }

    const session: AuthSession = {
      accessToken,
      accessTokenExpiresAtUtc,
      refreshToken,
      refreshTokenExpiresAtUtc,
    };

    try {
      this.authSession.completeExternalAuthentication(session);
      await this.authSession.loadCurrentUser();
      void this.router.navigateByUrl('/dashboard');
    } catch {
      this.errorMessage.set('Google girisi sonrasi kullanici bilgisi alinamadi.');
    }
  }
}
