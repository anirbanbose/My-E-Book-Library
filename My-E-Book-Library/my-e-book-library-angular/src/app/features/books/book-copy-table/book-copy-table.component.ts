import { Component, inject, Input, ViewChild } from '@angular/core';
import { BookService } from '../../../_services/book/book.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatTable, MatTableDataSource, MatTableModule } from '@angular/material/table';
import { BookCopyListItem } from '../book-copy-list/book-copy-list-item';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-book-copy-table',
  standalone: true,
  imports: [
    MatIconModule,
    MatTableModule,
    MatButtonModule,
  ],
  templateUrl: './book-copy-table.component.html',
  styleUrl: './book-copy-table.component.scss'
})
export class BookCopyTableComponent {
  @Input() dataSource: MatTableDataSource<any> = new MatTableDataSource();
  @ViewChild(MatTable) table!: MatTable<BookCopyListItem>;
  private _bookService = inject(BookService);
  private _snackBar = inject(MatSnackBar);
  displayedColumns = ['fileName', 'action'];

  downloadFile(id: number) {
    this._bookService.downloadFile(id).subscribe({
      next: (response: any) => {
        if (response && response.isSuccess) {
          const file = response.record;

          const binaryData = this.base64ToBlob(file.fileData, file.fileType);
          const url = window.URL.createObjectURL(binaryData);
          const a = document.createElement('a');
          a.href = url;
          a.download = file.fileName;
          document.body.appendChild(a);
          a.click();
          document.body.removeChild(a);
        }
        else {
          this._snackBar.open('There was an issue while downloading the book. Please try again later.', 'Close', {
            duration: 5000
          });
        }

      },
      error: (err) => {
        this._snackBar.open('There was an issue while downloading the book. Please try again later.', 'Close', {
          duration: 5000
        });
      }
    })
  }

  private base64ToBlob(base64: string, fileType: string): Blob {
    const byteCharacters = atob(base64);
    const byteNumbers = new Array(byteCharacters.length);
    for (let i = 0; i < byteCharacters.length; i++) {
      byteNumbers[i] = byteCharacters.charCodeAt(i);
    }
    const byteArray = new Uint8Array(byteNumbers);
    return new Blob([byteArray], { type: fileType });
  }


}
