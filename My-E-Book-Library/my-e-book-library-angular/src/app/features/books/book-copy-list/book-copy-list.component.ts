import { CommonModule } from '@angular/common';
import { Component, Inject, inject, OnInit, ViewChild } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogContent, MatDialogRef } from '@angular/material/dialog';
import { MatIcon } from '@angular/material/icon';
import { MatTable, MatTableDataSource, MatTableModule } from '@angular/material/table';
import { BookCopyListItem } from './book-copy-list-item';
import { BookService } from '../../../_services/book/book.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { BookCopyTableComponent } from '../book-copy-table/book-copy-table.component';

@Component({
  selector: 'app-book-copy-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatIcon,
    MatDialogActions,
    MatDialogContent,
    MatButtonModule,
    BookCopyTableComponent
  ],
  templateUrl: './book-copy-list.component.html',
  styleUrl: './book-copy-list.component.scss'
})
export class BookCopyListComponent implements OnInit {
  readonly dialogRef = inject(MatDialogRef<BookCopyListComponent>);
  dataSource: MatTableDataSource<any> = new MatTableDataSource();
  totalRecordCount: number = 0;
  private _bookService = inject(BookService);
  private _snackBar = inject(MatSnackBar);
  bookId: number = 0;
  bookTitle: string = '';

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
    if (data) {
      this.bookId = data.id;
    }
  }
  ngOnInit(): void {
    this.getBookCopyList();
  }

  getBookCopyList() {
    if (this.bookId > 0) {
      this._bookService.getBookCopies(this.bookId)
        .subscribe({
          next: (data: any) => {
            if (data?.isSuccess && data.record) {
              this.bookTitle = data.record.title;
              this.totalRecordCount = data.record.copies.length;
              this.dataSource = new MatTableDataSource(data.record.copies);
            }
            else {
              this.totalRecordCount = 0;
              this.dataSource = new MatTableDataSource();
            }
          },
          error: (err: any) => {
            this._snackBar.open('There was an issue while fetching the book copy list. Please try again later.', 'Close', {
              duration: 5000
            })
          }
        });
    }

  }

  onClose() {
    if (this.dialogRef) {
      this.dialogRef.close();
    }
  }


}
