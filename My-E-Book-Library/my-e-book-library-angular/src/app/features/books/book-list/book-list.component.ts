import { AfterViewInit, ChangeDetectionStrategy, Component, ElementRef, OnInit, ViewChild, inject } from '@angular/core';
import { MatTableModule, MatTable, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSortModule, MatSort, SortDirection, Sort } from '@angular/material/sort';
import { BookListItem } from './book-list-item';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { BookService } from '../../../_services/book/book.service';
import { HelperService } from '../../../_helpers/helper.service';
import { StorageService } from '../../../_services/storage.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, NavigationEnd, Params, Router } from '@angular/router';
import { ConfirmDialogData } from '../../shared/confirm-dialog/confirm-dialog-data';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';
import { MatCardModule } from '@angular/material/card';
import { BookCopyListComponent } from '../book-copy-list/book-copy-list.component';
import { BookListHeaderComponent } from '../book-list-header/book-list-header.component';
import { BookSearchFilter } from '../../../models/BookSearchFilter';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrl: './book-list.component.scss',
  standalone: true,
  imports: [
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatMenuModule,
    MatFormField,
    MatCardModule,
    MatLabel,
    MatInput,
    MatSuffix,
    MatIconModule,
    BookListHeaderComponent,
    FormsModule,
    MatDialogModule
  ]
})
export class BookListComponent implements AfterViewInit, OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  totalRecordCount: number = 0;
  private _bookService = inject(BookService);
  private _helperService = inject(HelperService);
  private _storageService = inject(StorageService);
  private _snackBar = inject(MatSnackBar);
  private _route = inject(ActivatedRoute);
  private _router = inject(Router);
  readonly dialog = inject(MatDialog);
  pageIndex: number = 0;
  pageSize: number = 10;
  sortField: string = 'Title';
  sortDirection: SortDirection = 'asc';
  searchText: string = '';
  authorId: number | null = null;
  publisherId: number | null = null;
  genreId: number | null = null;
  languageId: number | null = null;
  books: any[] = [];
  bookSearchFilter!: BookSearchFilter;

  constructor() {
    this._route.queryParams.subscribe(queryParams => {
      this.getValuesFromQueryParams(queryParams);
      if (this.searchText.trim() != '') {
        this.authorId = null;
        this.publisherId = null;
        this.genreId = null;
        this.languageId = null;
      }
      else {
        this.searchText = '';
      }

      this.bookSearchFilter = {
        searchText: this.searchText.trim(),
        authorId: this.authorId,
        publisherId: this.publisherId,
        genreId: this.genreId,
        languageId: this.languageId,
        sortColumn: this.sortField,
        isSortAscending: this.sortDirection == 'asc'
      }
    });
  }
  ngOnInit(): void {
    this.getBookList();
  }

  getValuesFromQueryParams(queryParams: Params) {
    this.searchText = queryParams["searchText"] || '';
    this.authorId = parseInt(queryParams["authorId"] as string) || null;
    this.publisherId = parseInt(queryParams["publisherId"] as string) || null;
    this.genreId = parseInt(queryParams["genreId"] as string) || null;
    this.languageId = parseInt(queryParams["languageId"] as string) || null;
  }

  ngAfterViewInit(): void {
    if (this.paginator) {
      //this.dataSource.paginator = this.paginator;
      this.paginator.page.subscribe((event: PageEvent) => {
        this.pageIndex = event.pageIndex;
        this.pageSize = event.pageSize;
        this.getBookList();
      });
    }
    /* if (this.sort) {
      //this.dataSource.sort = this.sort;
      this.sort.sortChange.subscribe((sort: Sort) => {
        this.sortField = sort.active;
        this.sortDirection = sort.direction;
        this.getBookList();
      });
    } */
  }

  getBookList() {
    this._bookService.bookList(this.pageIndex, this.pageSize, this.searchText, this._helperService.capitalizeFirstLetter(this.sortField), this.sortDirection === 'asc' ? 0 : 1, this.authorId, this.publisherId, this.genreId, this.languageId)
      .subscribe((data: any) => {
        if (data?.isSuccess) {
          this.totalRecordCount = data.totalRecordCount;
          this.books = data.records;
        }
        else {
          this.books = [];
          this.totalRecordCount = 0;
        }
      })
  }


  applyFilter(searchFilter: BookSearchFilter) {
    this.searchText = searchFilter.searchText;
    this.authorId = searchFilter.authorId;
    this.publisherId = searchFilter.publisherId;
    this.genreId = searchFilter.genreId;
    this.languageId = searchFilter.languageId;
    this.sortField = searchFilter.sortColumn;
    this.sortDirection = searchFilter.isSortAscending ? 'asc' : 'desc';
    const searchParams = this.searchText.trim() == ''
      ? { searchText: null, authorId: this.authorId, publisherId: this.publisherId, genreId: this.genreId, languageId: this.languageId }
      : { searchText: this.searchText, authorId: null, publisherId: null, genreId: null, languageId: null };
    this._helperService.navigateWithSearchParams(searchParams);
    this.getBookList();
  }

  deleteBook(id: number) {
    let data: ConfirmDialogData = {
      title: 'Delete Book?',
      message: 'Are you sure you want to delete this book?',
      OkButtonText: "Yes",
      cancelButtonText: "No"
    }
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      height: '200px',
      width: '400px',
      data: data
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this._bookService.deleteBook(id).subscribe({
          next: () => {
            this.getBookList();
            this._snackBar.open('Book successfully deleted.', 'Close', {
              duration: 5000
            })
          },
          error: (err) => {
            this._snackBar.open('There was an issue deleting the book. Please try again later.', 'Close', {
              duration: 5000
            })
          }
        });
      }
    });

  }
  goToEditBook(id: number) {
    this._router.navigate([`/edit-book/${id}`]);
  }
  goToBookDetail(id: number) {
    this._router.navigate([`/book-detail/${id}`]);
  }
  addNewBook() {
    this._router.navigate(['/add-book'])
  }

  get currentUser(): any {
    return this._storageService.getLoggedInUser();
  }

  openBookCopyDialog(bookId: number) {
    const dialogRef = this.dialog.open(BookCopyListComponent, {
      data: { id: bookId },
      height: '350px',
      width: '600px',
    });
  }

}
