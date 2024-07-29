import { Component, inject, signal } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../../_services/auth/auth.service';
import { Router, RouterLink } from '@angular/router';
import { StorageService } from '../../_services/storage.service';
import { EmailValidator } from '../../custom-validators/email-validator';
import { AccountService } from '../../_services/account/account.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpErrorResponse } from '@angular/common/http';
import { StrongPasswordRegx } from '../../_helpers/regex';
import { passwordsMatchValidator } from '../../custom-validators/password-match-validator';
import { HelperService } from '../../_helpers/helper.service';

@Component({
  selector: 'app-registration',
  standalone: true,
  imports: [
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,
    ReactiveFormsModule,
    MatIconModule,
    RouterLink
  ],
  templateUrl: './registration.component.html',
  styleUrl: './registration.component.scss'
})
export class RegistrationComponent {
  private fb = inject(FormBuilder);
  private _accountService = inject(AccountService);
  private router = inject(Router);
  private _snackBar = inject(MatSnackBar);
  private _helperService = inject(HelperService);
  hidePassword = signal(true);
  hideConfirmPassword = signal(true);
  errorMessages: string[] = [];

  registrationForm: FormGroup;



  constructor() {
    this.registrationForm = this.fb.group({
      firstName: [null, [Validators.required, Validators.maxLength(100)]],
      middleName: [null, Validators.maxLength(100)],
      lastName: [null, [Validators.required, Validators.maxLength(100)]],
      email: [null, [Validators.required, Validators.maxLength(250), Validators.email], [EmailValidator.createValidator(this._accountService)]],
      password: [null, [Validators.required, Validators.maxLength(20), , Validators.minLength(8), Validators.pattern(StrongPasswordRegx)]],
      confirmPassword: [null, Validators.required]
    });
    this.registrationForm.addValidators(passwordsMatchValidator());
  }

  onSubmit() {
    this.errorMessages = [];
    if (!this.registrationForm.valid) {
      return;
    }
    let formData: any = this.registrationForm.value;
    this._accountService.register(formData).subscribe((data: any) => {
      if (data?.isSuccess) {
        this._snackBar.open("Registration successful.", 'Close', {
          duration: 5000
        });
        this.router.navigate(['/account/login']);
      }
      else if (data?.error) {
        this._snackBar.open(data.error.errorMessage, 'Close', {
          duration: 5000
        });
      }
      else {
        this._snackBar.open("There was an issue in the registration process. Please try again later", 'Close', {
          duration: 5000
        })
      }
    },
      (err: HttpErrorResponse) => {
        if (err.status == 422) {
          this.errorMessages = this._helperService.getErrorMessages(err);
        }
        else {
          this._snackBar.open("There was an issue in the registration process. Please try again later", 'Close', {
            duration: 5000
          })
        }
      });
  }

  togglePasswordVisibility($event: any) {
    this.hidePassword.set(!this.hidePassword());
    $event.stopPropagation();
  }

  toggleConfirmPasswordVisibility($event: any) {
    this.hideConfirmPassword.set(!this.hideConfirmPassword());
    $event.stopPropagation();
  }

}
