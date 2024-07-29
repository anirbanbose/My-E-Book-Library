import { Component, Input, ViewChild, ElementRef, AfterViewInit, output } from '@angular/core';
import { MatIcon } from '@angular/material/icon';
import { DragNDropDirective } from '../../../_directives/dragNDrop/drag-ndrop.directive';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-file-upload',
  standalone: true,
  imports: [
    CommonModule,
    MatInputModule,
    MatIcon,
    DragNDropDirective,
    MatButtonModule
  ],
  templateUrl: './file-upload.component.html',
  styleUrl: './file-upload.component.scss'
})
export class FileUploadComponent implements AfterViewInit {


  ngAfterViewInit(): void {
    if (this.allowMultiple) {
      this.fileDropRef.nativeElement.multiple = true
    }
    else {
      this.fileDropRef.nativeElement.multiple = false
    }
  }

  @Input() fileHint: string = '';
  @Input() uploaderText: string = 'Drag & Drop File(s) Here';
  @Input() allowMultiple: boolean = false;
  @Input() allowedFileTypes: string[] = [];
  @Input() maxFileSizeInMB: number = 0;
  onFileAdded = output<any>();
  onFileValidationError = output();

  @ViewChild('fileDropRef') fileDropRef!: ElementRef<HTMLInputElement>;

  onFileDropped($event: any) {
    this.processFiles($event);
  }

  fileBrowseHandler($event: any) {
    this.processFiles($event.target.files);
  }


  processFiles(fileList: FileList) {
    let hasValidationError: boolean = false;
    let files: any[] = [];
    for (let i = 0; i < fileList.length; i++) {
      if (this.validateFile(fileList[i])) {
        files.push(fileList[i]);
      }
      else {
        hasValidationError = true;
      }
    }
    if (hasValidationError) {
      this.onFileValidationError.emit();
    }
    if (files.length > 0) {
      this.onFileAdded.emit(files);
    }
  }


  validateFile(file: any) {
    if (this.allowedFileTypes.length > 0 && !this.allowedFileTypes.includes(file.type)) {
      return false;
    }
    if (!isNaN(this.maxFileSizeInMB)) {
      const maxSizeInBytes: number = (this.maxFileSizeInMB as number) * 1024 * 1024;
      if (file.size > maxSizeInBytes) {
        return false;
      }
    }
    return true;
  }
}
