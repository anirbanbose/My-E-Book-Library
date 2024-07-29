import { Component, inject, OnInit } from '@angular/core';
import { BookService } from '../../../_services/book/book.service';
import { HelperService } from '../../../_helpers/helper.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FeaturePageHeaderComponent } from '../../shared/feature-page-header/feature-page-header.component';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NgxMaskPipe, provideNgxMask } from 'ngx-mask';
import { MatTableDataSource } from '@angular/material/table';
import { BookCopyTableComponent } from '../book-copy-table/book-copy-table.component';

@Component({
  selector: 'app-book-detail',
  standalone: true,
  imports: [
    FeaturePageHeaderComponent,
    MatCardModule,
    NgxMaskPipe,
    BookCopyTableComponent
  ],
  providers: [
    provideNgxMask()
  ],
  templateUrl: './book-detail.component.html',
  styleUrl: './book-detail.component.scss'
})
export class BookDetailComponent implements OnInit {
  private _bookService = inject(BookService);
  private _helperService = inject(HelperService);
  private _route = inject(ActivatedRoute);
  private _router = inject(Router); id: number = 0;
  private _snackBar = inject(MatSnackBar);
  fileDataSource: MatTableDataSource<any> = new MatTableDataSource();
  headerText: string = 'Book Detail';
  book: any = {};

  constructor() {
    if (this._route.snapshot.paramMap.get('id')) {
      this.id = parseInt(this._route.snapshot.paramMap.get('id') as string);
    }
  }
  ngOnInit(): void {
    this.getBook();
  }

  getBook() {
    if (this.id > 0) {

      this._bookService.getBookDetail(this.id).subscribe({
        next: (response: any) => {
          if (response && response.isSuccess) {
            this.book = response.record;
            this.fileDataSource = new MatTableDataSource(this.book.files);

            /* book.authors.map((author: any) => {
              const authorControl = new FormGroup({
                'authorId': new FormControl(author.id),
                'authorName': new FormControl(author.authorName),
                'authorTypeId': new FormControl(author.authorTypeId)
              });
              (<FormArray>this.authors).push(authorControl);
            }); */
            /* this.ebooks = book.files.map((file: any) => {
              return {
                id: file.id,
                name: file.fileName,
                size: file.fileSize,
                type: file.fileType
              }
            }) */

          }
          else {
            this._snackBar.open("There was an error in retrieving the book record. Please try again later", 'Close', {
              duration: 5000
            });
            this._router.navigate(['/books']);
          }
        },
        error: (err: any) => {
          this._snackBar.open("There was an error in retrieving the book record. Please try again later", 'Close', {
            duration: 5000
          });
          this._router.navigate(['/books']);
        }
      })
    }
  }

}
