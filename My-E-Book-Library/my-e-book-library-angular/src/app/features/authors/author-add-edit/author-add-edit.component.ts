import { Component, ElementRef, Inject, OnInit, ViewChild, inject } from '@angular/core';
import { FormBuilder, NgForm, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { AuthorService } from '../../../_services/author/author.service';
import { AuthService } from '../../../_services/auth/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HelperService } from '../../../_helpers/helper.service';

@Component({
  selector: 'app-author-add-edit',
  standalone: true,
  imports: [
    MatInputModule,
    MatButtonModule,
    MatCheckboxModule,
    MatCardModule,
    ReactiveFormsModule,
    MatIconModule,
    MatDialogActions,
    MatDialogContent,
    MatDialogTitle
  ],
  templateUrl: './author-add-edit.component.html',
  styleUrl: './author-add-edit.component.scss'
})
export class AuthorAddEditComponent implements OnInit {
  @ViewChild('authorFormRef') authorFormRef!: NgForm;

  readonly dialogRef = inject(MatDialogRef<AuthorAddEditComponent>);
  private fb = inject(FormBuilder);
  private _authorService = inject(AuthorService);
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

  authorForm = this.fb.group({
    id: [0],
    firstName: [null, [Validators.required, Validators.maxLength(100)]],
    middleName: [null, Validators.maxLength(100)],
    lastName: [null, [Validators.required, Validators.maxLength(100)]]
  });

  ngOnInit(): void {
    if (this.id > 0) {
      this.getAuthor();
    }
  }


  onSubmit() {
    this.errorMessages = [];
    if (!this.authorForm.valid) {
      return;
    }
    this._authorService.saveAuthor(this.authorForm.value).subscribe({
      next: (data: any) => {
        if (data?.isSuccess) {
          this.dialogRef.close(data?.savedValue);
          this._snackBar.open("Author saved successfully.", 'Close', {
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
          this.dialogRef.updateSize('400px', '550px');
        }
        else {
          this._snackBar.open("There was an issue while saving the record. Please try again later", 'Close', {
            duration: 5000
          });
        }
      }
    });
  }

  getAuthor() {
    this._authorService.getAuthor(this.id).subscribe({
      next: (data: any) => {
        if (data?.isSuccess) {
          this.authorForm.patchValue({
            id: data.record.id,
            firstName: data.record.firstName,
            middleName: data.record.middleName,
            lastName: data.record.lastName
          })
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
