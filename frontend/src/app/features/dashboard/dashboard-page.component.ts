import { Component } from '@angular/core';

@Component({
  selector: 'app-dashboard-page',
  template: `
    <section class="mx-auto max-w-7xl px-6 py-16">
      <div class="grid gap-6 md:grid-cols-3">
        <div class="rounded-3xl border border-white/10 bg-white/5 p-6 md:col-span-2">
          <p class="text-sm font-semibold text-cyan-200">Dashboard Module</p>
          <h1 class="mt-3 text-3xl font-semibold text-white">Kariyer metrikleri icin bos ama hazir alan</h1>
          <p class="mt-4 max-w-2xl text-slate-300">
            Match score, basvuru ozeti, skill gap ve AI araclari daha sonra burada toparlanabilir.
          </p>
        </div>

        <div class="rounded-3xl border border-white/10 bg-slate-900/90 p-6">
          <p class="text-sm font-semibold text-slate-300">Sprint Notes</p>
          <ul class="mt-4 space-y-3 text-sm text-slate-400">
            <li>Standalone component tabanli</li>
            <li>Tailwind ile stil omurgasi hazir</li>
            <li>Feature bazli route yapisi tanimli</li>
          </ul>
        </div>
      </div>
    </section>
  `,
})
export class DashboardPageComponent {}
