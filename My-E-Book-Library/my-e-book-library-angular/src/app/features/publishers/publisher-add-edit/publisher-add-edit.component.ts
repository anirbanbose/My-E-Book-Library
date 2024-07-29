import { Component, Inject, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { PublisherService } from '../../../_services/publisher/publisher.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../_services/auth/auth.service';
import { HelperService } from '../../../_helpers/helper.service';

@Component({
  selector: 'app-publisher-add-edit',
  standalone: true,
  imports: [
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,
    ReactiveFormsModule,
    MatDatepickerModule,
    MatIconModule,
    MatDialogActions,
    MatDialogContent,
    MatDialogTitle
  ],
  templateUrl: './publisher-add-edit.component.html',
  styleUrl: './publisher-add-edit.component.scss'
})
export class PublisherAddEditComponent implements OnInit {

  readonly dialogRef = inject(MatDialogRef<PublisherAddEditComponent>);
  private fb = inject(FormBuilder);
  private _publisherService = inject(PublisherService);
  private _authService = inject(AuthService);
  private _helperService = inject(HelperService);
  private _snackBar = inject(MatSnackBar);
  mode: string = 'Add';
  id: number = 0;
  errorMessages: string[] = [];

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
    if (data) {
      this.id = data.id;
      this.mode = "Edit";
    }
  }

  publisherForm = this.fb.group({
    id: [0],
    publisherName: [null, [Validators.required, Validators.maxLength(150)]]
  });

  ngOnInit(): void {
    if (this.id > 0) {
      this.getPublisher();
    }
  }

  onSubmit() {
    this.errorMessages = [];
    if (!this.publisherForm.valid) {
      return;
    }
    this._publisherService.savePublisher(this.publisherForm.value).subscribe({
      next: (data: any) => {
        if (data?.isSuccess) {
          this.dialogRef.close(data?.savedValue);
          this._snackBar.open("Publisher saved successfully.", 'Close', {
            duration: 5000
          });
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
      error: (err: any) => {
        if (err.status == 401) {
          this._snackBar.open("You are not authorized to perform this action.", 'Close', {
            duration: 5000
          });
          this.dialogRef.close();
          this._authService.logout();
        }
        else if (err.status == 422) {
          this.errorMessages = this._helperService.getErrorMessages(err);
          this.dialogRef.updateSize('400px', '350px');
        }
        else {
          this._snackBar.open("There was an issue while saving the record. Please try again later", 'Close', {
            duration: 5000
          });
        }
      }
    });
  }

  getPublisher() {
    this._publisherService.getPublisher(this.id).subscribe({
      next: (data: any) => {
        if (data?.isSuccess) {
          this.publisherForm.get('id')?.setValue(data.record.id);
          this.publisherForm.get('publisherName')?.setValue(data.record.publisherName);
        }
        else if (data?.error) {
          this._snackBar.open(data.error.errorMessage, 'Close', {
            duration: 5000
          });
        }
        else {
          this._snackBar.open("There was an issue while fetching the record. Please try again later", 'Close', {
            duration: 5000
          });
        }
      },
      error: (err: any) => {
        this._snackBar.open("There was an issue while fetching the record. Please try again later", 'Close', {
          duration: 5000
        });
      }
    })
  }

  onCancel() {
    this.dialogRef.close();
  }
}
