import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { ProfileService } from '../../../_services/profile/profile.service';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatCardModule } from '@angular/material/card';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../../../_services/auth/auth.service';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { FeaturePageHeaderComponent } from '../../shared/feature-page-header/feature-page-header.component';
import { provideNativeDateAdapter } from '@angular/material/core';
import { catchError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HelperService } from '../../../_helpers/helper.service';
import { MatIconModule } from '@angular/material/icon';
import { StrongPasswordRegx } from '../../../_helpers/regex';
import { passwordsMatchValidator } from '../../../custom-validators/password-match-validator';

@Component({
  selector: 'app-profile',
  standalone: true,
  providers: [provideNativeDateAdapter()],
  imports: [
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    FeaturePageHeaderComponent,
    MatIconModule
  ],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProfileComponent implements OnInit {
  private fb = inject(FormBuilder);
  private _authService = inject(AuthService);
  private _profileService = inject(ProfileService);
  private _snackBar = inject(MatSnackBar);
  private _helperService = inject(HelperService);
  hideCurrentPassword = signal(true);
  hideNewPassword = signal(true);
  hideConfirmNewPassword = signal(true);
  passwordChangeAttemptCount: number = 0;

  profileForm = this.fb.group({
    id: [0],
    firstName: [null, [Validators.required, Validators.maxLength(100)]],
    middleName: [null, Validators.maxLength(100)],
    lastName: [null, [Validators.required, Validators.maxLength(100)]],
    email: [{ value: null, disabled: true }, [Validators.required, Validators.email]],
    birthDate: [null]
  });

  changePasswordForm = this.fb.group({
    oldPassword: [null, Validators.required],
    newPassword: [null, [Validators.required, Validators.maxLength(20), , Validators.minLength(8), Validators.pattern(StrongPasswordRegx)]],
    confirmNewPassword: [null, Validators.required],
  });

  constructor() {
    this.changePasswordForm.addValidators(passwordsMatchValidator());
  }

  ngOnInit(): void {
    this.getProfile();
  }

  getProfile() {
    this._profileService.profile().subscribe((data: any) => {
      if (data?.isSuccess) {
        this.profileForm.patchValue({
          id: data.record.id,
          firstName: data.record.firstName,
          middleName: data.record.middleName,
          lastName: data.record.lastName,
          email: data.record.email,
          birthDate: data.record.birthDate
        })

      }
      else if (data?.error) {
        this._snackBar.open(data?.error.errorMessage, 'Close', {
          duration: 5000
        });
      }
      else {
        this._snackBar.open("There was an issue fetching the record. Please try again later", 'Close', {
          duration: 5000
        });
      }
    },
      (err: HttpErrorResponse) => {
        if (err.status == 401) {
          this._snackBar.open("You are not authorized to see this page.", 'Close', {
            duration: 5000
          });
          this._authService.logout();
        }
        else {
          this._snackBar.open("There was an issue fetching the record. Please try again later", 'Close', {
            duration: 5000
          });
        }
      })
  }

  onSubmit() {
    if (!this.profileForm.valid) {
      return;
    }
    let formData: any = this.profileForm.value;
    formData.birthDate = formData.birthDate ? this._helperService.dateWithoutTimezone(new Date(formData.birthDate)) : null;
    this._profileService.saveProfile(formData).subscribe((data: any) => {
      if (data?.isSuccess) {
        this._snackBar.open("Profile saved successfully.", 'Close', {
          duration: 5000
        });
        this._authService.saveUser(data.savedRecord);
      }
      else if (data?.error) {
        this._snackBar.open(data.error.errorMessage, 'Close', {
          duration: 5000
        });
      }
      else {
        this._snackBar.open("There was an issue while saving the record. Please try again later", 'Close', {
          duration: 5000
        });
      }
    },
      (err: HttpErrorResponse) => {
        if (err.status == 401) {
          this._snackBar.open("You are not authorized to perform this action.", 'Close', {
            duration: 5000
          });
          this._authService.logout();
        }
        else {
          this._snackBar.open("There was an issue while saving the record. Please try again later", 'Close', {
            duration: 5000
          });
        }
      });

  }

  onPasswordChange() {
    if (!this.changePasswordForm.valid) {
      return;
    }
    let model: any = {
      oldPassword: this.changePasswordForm.value.oldPassword,
      newPassword: this.changePasswordForm.value.newPassword
    }

    this._profileService.changePassword(model).subscribe((data: any) => {
      if (data?.isSuccess) {
        this.markFormAsValid();
        this._snackBar.open("Password changed successfully.", 'Close', {
          duration: 5000
        });
      }
      else {
        this.passwordChangeAttemptCount += 1;
        if (this.passwordChangeAttemptCount < 3 && data?.error) {
          this._snackBar.open(data.error.errorMessage, 'Close', {
            duration: 5000
          });
        }
        else {
          this._authService.logout();
        }

      }
    },
      (err: HttpErrorResponse) => {
        if (err.status == 401) {
          this._snackBar.open("You are not authorized to perform this action.", 'Close', {
            duration: 5000
          });
          this._authService.logout();
        }
        else {
          this._snackBar.open("There was an issue while changing the password. Please try again later.", 'Close', {
            duration: 5000
          });
        }
      });
  }
  markFormAsValid() {
    this.changePasswordForm.reset();

    this.changePasswordForm.markAsPristine();
    this.changePasswordForm.markAsUntouched();
    this.changePasswordForm.setErrors(null);

    Object.keys(this.changePasswordForm.controls).forEach(key => {
      this.changePasswordForm?.get(key)?.setErrors(null);
    });
  }


  toggleCurrentPassword(event: MouseEvent) {
    this.hideCurrentPassword.set(!this.hideCurrentPassword());
    event.stopPropagation();
  }

  toggleNewPassword(event: MouseEvent) {
    this.hideNewPassword.set(!this.hideNewPassword());
    event.stopPropagation();
  }
  toggleConfirmNewPassword(event: MouseEvent) {
    this.hideConfirmNewPassword.set(!this.hideConfirmNewPassword());
    event.stopPropagation();
  }
}
