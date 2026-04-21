import { HttpErrorResponse } from '@angular/common/http';
import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { environment } from '../../../../environments/environment';
import { UserRole } from '../../../core/auth/models/auth.models';
import { AuthSessionService } from '../../../core/auth/services/auth-session.service';
import { GoogleAuthButtonComponent } from '../../../shared/ui/google-auth-button/google-auth-button.component';

@Component({
  selector: 'app-register-page',
  imports: [ReactiveFormsModule, RouterLink, GoogleAuthButtonComponent],
  templateUrl: './register-page.component.html',
})
export class RegisterPageComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authSession = inject(AuthSessionService);
  private readonly router = inject(Router);

  protected readonly isSubmitting = signal(false);
  protected readonly submitError = signal<string | null>(null);
  protected readonly googleCandidateUrl = `${environment.apiOrigin}/api/auth/google/start?role=Candidate`;
  protected readonly googleEmployerUrl = `${environment.apiOrigin}/api/auth/google/start?role=Employer`;

  protected readonly roleCards: Array<{ value: Exclude<UserRole, 'Admin'>; title: string; text: string }> = [
    {
      value: 'Candidate',
      title: 'Aday',
      text: 'CV, basvuru ve AI kariyer araclarini kullanmak isteyen profesyoneller icin.',
    },
    {
      value: 'Employer',
      title: 'Isveren',
      text: 'Adaylari degerlendirmek, match score ve ise alim akislarini yonetmek isteyen ekipler icin.',
    },
  ];

  protected readonly form = this.formBuilder.nonNullable.group({
    firstName: ['', [Validators.required, Validators.maxLength(100)]],
    lastName: ['', [Validators.required, Validators.maxLength(100)]],
    email: ['', [Validators.required, Validators.email]],
    role: ['Candidate' as Exclude<UserRole, 'Admin'>, [Validators.required]],
    password: ['', [Validators.required, Validators.minLength(8)]],
    confirmPassword: ['', [Validators.required]],
  });

  protected readonly passwordMismatch = computed(() => {
    const { password, confirmPassword } = this.form.getRawValue();
    return this.form.controls.confirmPassword.touched && password !== confirmPassword;
  });

  submit(): void {
    if (this.form.invalid || this.passwordMismatch() || this.isSubmitting()) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.submitError.set(null);

    this.authSession
      .register(this.form.getRawValue())
      .then(() => this.router.navigateByUrl('/dashboard'))
      .catch((error: HttpErrorResponse) => {
        const validationErrors = error.error?.errors as Record<string, string[]> | undefined;
        const detail = error.error?.detail as string | undefined;

        if (validationErrors) {
          const firstError = Object.values(validationErrors).flat()[0];
          this.submitError.set(firstError ?? 'Kayit olusturulamadi.');
          return;
        }

        this.submitError.set(detail ?? 'Kayit olusturulurken bir sorun olustu.');
      })
      .finally(() => {
        this.isSubmitting.set(false);
      });
  }

  protected selectRole(role: Exclude<UserRole, 'Admin'>): void {
    this.form.controls.role.setValue(role);
    this.form.controls.role.markAsDirty();
  }

  protected fieldError(fieldName: 'firstName' | 'lastName' | 'email' | 'password' | 'confirmPassword'): string | null {
    const control = this.form.controls[fieldName];

    if (!control.touched || !control.invalid) {
      return null;
    }

    if (control.hasError('required')) {
      return 'Bu alan zorunludur.';
    }

    if (control.hasError('email')) {
      return 'Gecerli bir e-posta girin.';
    }

    if (control.hasError('minlength')) {
      return 'En az 8 karakter olmali.';
    }

    if (control.hasError('maxlength')) {
      return 'Maksimum karakter sinirini astiniz.';
    }

    return null;
  }
}
