import { DOCUMENT } from '@angular/common';
import { Injectable, inject } from '@angular/core';
import { Meta, Title } from '@angular/platform-browser';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter } from 'rxjs';

import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class SeoService {
  private readonly router = inject(Router);
  private readonly title = inject(Title);
  private readonly meta = inject(Meta);
  private readonly document = inject(DOCUMENT);
  private initialized = false;

  initialize(): void {
    if (this.initialized) {
      return;
    }

    this.initialized = true;

    this.router.events
      .pipe(filter((event): event is NavigationEnd => event instanceof NavigationEnd))
      .subscribe(() => {
        const route = this.getLeafRoute(this.router.routerState.root);
        const title = route.snapshot.data['title'] as string | undefined;
        const description = route.snapshot.data['description'] as string | undefined;
        const canonical = route.snapshot.data['canonical'] as string | undefined;

        if (title) {
          this.title.setTitle(title);
        }

        if (description) {
          this.meta.updateTag({ name: 'description', content: description });
        }

        this.updateCanonical(canonical ?? this.router.url);
      });
  }

  private getLeafRoute(route: ActivatedRoute): ActivatedRoute {
    let current = route;

    while (current.firstChild) {
      current = current.firstChild;
    }

    return current;
  }

  private updateCanonical(path: string): void {
    const href = path.startsWith('http') ? path : `${environment.siteUrl}${path === '/' ? '' : path}`;
    let link = this.document.querySelector('link[rel="canonical"]') as HTMLLinkElement | null;

    if (!link) {
      link = this.document.createElement('link');
      link.setAttribute('rel', 'canonical');
      this.document.head.appendChild(link);
    }

    link.setAttribute('href', href);
  }
}
