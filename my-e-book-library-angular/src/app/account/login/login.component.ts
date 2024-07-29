import { Component, inject, signal } from '@angular/core';

import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { AuthService } from '../../_services/auth/auth.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { StorageService } from '../../_services/storage.service';
import { MatIconModule } from '@angular/material/icon';
import { HelperService } from '../../_helpers/helper.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  standalone: true,
  imports: [
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,
    ReactiveFormsModule,
    MatIconModule,
    RouterLink
  ]
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private _authService = inject(AuthService);
  private router = inject(Router);
  private _localStorage = inject(StorageService);
  private _helperService = inject(HelperService);
  hide = signal(true);
  errorMessages: string[] = [];

  loginForm = this.fb.group({
    email: [null, [Validators.required, Validators.email]],
    password: [null, Validators.required],
    rememberMe: null
  });

  returnUrl: string | null = null;
  hasUnitNumber = false;

  onSubmit(): void {
    this.errorMessages = [];
    let _loginModel: any = {
      email: this.loginForm.get('email')?.value,
      password: this.loginForm.get('password')?.value,
      rememberMe: false,
      deviceId: this._localStorage.getDeviceId(),
      userAgent: navigator.userAgent
    };
    this._authService.login(_loginModel).subscribe(
      (data: any) => {
        if (data) {
          if (this.returnUrl && this.returnUrl.trim() != '') {
            window.location.href = this.returnUrl;
          }
          else {
            this.router.navigate(['/dashboard']);
          }
        }
      },
      (err: HttpErrorResponse) => {
        if (err.status == 422) {
          this.errorMessages = this._helperService.getErrorMessages(err);
        }
        else {
          this.errorMessages = ["There was an issue in the registration process. Please try again later"];
        }
      }
    );
  }

  clickEvent(event: MouseEvent) {
    this.hide.set(!this.hide());
    event.stopPropagation();
  }
}
