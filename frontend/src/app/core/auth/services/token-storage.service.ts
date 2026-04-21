import { Injectable } from '@angular/core';

import { AuthSession } from '../models/auth.models';

@Injectable({ providedIn: 'root' })
export class TokenStorageService {
  private readonly storageKey = 'kariyeros.auth';

  getSession(): AuthSession | null {
    if (typeof localStorage === 'undefined') {
      return null;
    }

    const value = localStorage.getItem(this.storageKey);

    if (!value) {
      return null;
    }

    try {
      return JSON.parse(value) as AuthSession;
    } catch {
      localStorage.removeItem(this.storageKey);
      return null;
    }
  }

  setSession(session: AuthSession): void {
    localStorage.setItem(this.storageKey, JSON.stringify(session));
  }

  clear(): void {
    localStorage.removeItem(this.storageKey);
  }
}
