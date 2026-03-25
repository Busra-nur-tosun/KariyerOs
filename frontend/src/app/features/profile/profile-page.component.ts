import { Component } from '@angular/core';

@Component({
  selector: 'app-profile-page',
  template: `
    <section class="mx-auto max-w-5xl px-6 py-16">
      <div class="rounded-3xl border border-white/10 bg-white/5 p-8">
        <p class="text-sm font-semibold text-cyan-200">Profile Module</p>
        <h1 class="mt-3 text-3xl font-semibold text-white">Kullanici ve profil yonetimi icin hazir alan</h1>
        <p class="mt-4 max-w-2xl text-slate-300">
          Hesap ayarlari, temel profil, CV listesi ve tercihler bu bolume eklenecek.
        </p>
      </div>
    </section>
  `,
})
export class ProfilePageComponent {}
