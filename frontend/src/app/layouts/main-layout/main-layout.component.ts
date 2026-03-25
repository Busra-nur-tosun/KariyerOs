import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-main-layout',
  imports: [RouterOutlet, RouterLink],
  templateUrl: './main-layout.component.html',
})
export class MainLayoutComponent {
  // Navigation tek yerde tutuluyor.
  // Yeni moduller eklendiginde menuye ekleme yapmak kolay olsun diye dizi yapisi tercih edildi.
  protected readonly navigationItems = [
    { label: 'Adaylar icin', fragment: 'features' },
    { label: 'Isverenler icin', fragment: 'match-score' },
    { label: 'Yorumlar', fragment: 'results' },
    { label: 'AI Ozellikleri', fragment: 'features' },
    { label: 'Kaynaklar', fragment: 'resources-preview' },
    { label: 'Fiyatlandirma', fragment: 'pricing' },
  ];
}
