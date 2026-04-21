import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import { environment } from '../../../../environments/environment';
import { AuthSessionService } from '../../../core/auth/services/auth-session.service';
import { GoogleAuthButtonComponent } from '../../../shared/ui/google-auth-button/google-auth-button.component';

@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule, RouterLink, GoogleAuthButtonComponent],
  templateUrl: './login-page.component.html',
})
export class LoginPageComponent {
  private readonly formBuilder = inject(FormBuilder);
  private readonly authSession = inject(AuthSessionService);
  private readonly router = inject(Router);

  protected readonly isSubmitting = signal(false);
  protected readonly submitError = signal<string | null>(null);
  protected readonly googleLoginUrl = `${environment.apiOrigin}/api/auth/google/start?role=Candidate`;

  protected readonly form = this.formBuilder.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]],
  });

  submit(): void {
    if (this.form.invalid || this.isSubmitting()) {
      this.form.markAllAsTouched();
      return;
    }

    this.isSubmitting.set(true);
    this.submitError.set(null);

    this.authSession
      .login(this.form.getRawValue())
      .then(() => this.router.navigateByUrl('/dashboard'))
      .catch((error: HttpErrorResponse) => {
        this.submitError.set(error.error?.detail ?? 'Giris yapilirken bir sorun olustu.');
      })
      .finally(() => {
        this.isSubmitting.set(false);
      });
  }

  protected fieldError(fieldName: 'email' | 'password'): string | null {
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

    return null;
  }
}
