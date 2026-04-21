import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-google-auth-button',
  templateUrl: './google-auth-button.component.html',
})
export class GoogleAuthButtonComponent {
  @Input({ required: true }) href = '';
  @Input() label = 'Google ile devam et';
}
