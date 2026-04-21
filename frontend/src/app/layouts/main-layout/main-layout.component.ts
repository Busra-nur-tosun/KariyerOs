import { Component, computed, inject } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

import { AuthSessionService } from '../../core/auth/services/auth-session.service';

@Component({
  selector: 'app-main-layout',
  imports: [RouterOutlet, RouterLink],
  templateUrl: './main-layout.component.html',
})
export class MainLayoutComponent {
  private readonly authSession = inject(AuthSessionService);

  // Navigation tek yerde tutuluyor.
  // Yeni moduller eklendiginde menuye ekleme yapmak kolay olsun diye dizi yapisi tercih edildi.
  protected readonly navigationItems = [
    { label: 'Adaylar için', fragment: 'features' },
    { label: 'İşverenler için', fragment: 'match-score' },
    { label: 'Yorumlar', fragment: 'results' },
    { label: 'AI Özellikleri', fragment: 'features' },
    { label: 'Kaynaklar', fragment: 'resources-preview' },
    { label: 'Fiyatlandırma', fragment: 'pricing' },
  ];

  protected readonly currentUser = this.authSession.currentUser;
  protected readonly isAuthenticated = computed(() => this.authSession.isAuthenticated());

  protected logout(): void {
    this.authSession.logout('/');
  }
}
