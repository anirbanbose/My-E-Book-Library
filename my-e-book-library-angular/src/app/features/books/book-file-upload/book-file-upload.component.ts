import { Component, Inject, inject } from '@angular/core';
import { FileUploadComponent } from '../../shared/file-upload/file-upload.component';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatError } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { BookFileUploadData } from './book-file-upload-data';

@Component({
  selector: 'app-book-file-upload',
  standalone: true,
  imports: [
    CommonModule,
    FileUploadComponent,
    MatDialogModule,
    MatButtonModule,
    MatError,
  ],
  templateUrl: './book-file-upload.component.html',
  styleUrl: './book-file-upload.component.scss'
})
export class BookFileUploadComponent {
  readonly dialogRef = inject(MatDialogRef<BookFileUploadComponent>);
  title: string = '';
  maxFileSize: number = 0;
  allowMultiple: boolean = false;
  fileUploadError: string = '';
  uploaderText: string = '';
  fileHint: string = '';
  allowedFileTypes: string[] = [];

  constructor(@Inject(MAT_DIALOG_DATA) private data: BookFileUploadData) {
    if (data) {
      this.title = data.title;
      this.maxFileSize = data.maxFileSize;
      this.allowMultiple = data.allowMultiple;
      this.uploaderText = data.uploaderText;
      this.fileHint = data.fileHint;
      this.allowedFileTypes = data.allowedFileTypes;
    }
  }

  onDismiss() {
    this.dialogRef.close();
  }

  fileAdded(files: any[]) {
    this.fileUploadError = '';
    this.dialogRef.close(files);
  }

  onFileValidationError() {
    this.dialogRef.updateSize('500px', '520px');
    this.fileUploadError = this.fileHint;
  }
}
