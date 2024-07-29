import { AfterViewInit, Component, OnInit, ViewChild, inject } from '@angular/core';
import { MatTableModule, MatTable, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSortModule, MatSort, SortDirection, Sort } from '@angular/material/sort';
import { AuthorListItem } from './author-list-item';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatIcon } from '@angular/material/icon';
import { FeaturePageHeaderComponent } from '../../shared/feature-page-header/feature-page-header.component';
import { FormsModule } from '@angular/forms';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AuthorService } from '../../../_services/author/author.service';
import { HelperService } from '../../../_helpers/helper.service';
import { StorageService } from '../../../_services/storage.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { ConfirmDialogData } from '../../shared/confirm-dialog/confirm-dialog-data';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';
import { AuthorAddEditComponent } from '../author-add-edit/author-add-edit.component';

@Component({
  selector: 'app-author-list',
  templateUrl: './author-list.component.html',
  styleUrl: './author-list.component.scss',
  standalone: true,
  imports: [
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatButtonModule,
    MatMenuModule,
    MatFormField,
    MatLabel,
    MatInput,
    MatSuffix,
    MatIcon,
    FeaturePageHeaderComponent,
    FormsModule,
    MatDialogModule
  ]
})
export class AuthorListComponent implements AfterViewInit, OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<AuthorListItem>;

  dataSource: MatTableDataSource<any> = new MatTableDataSource();
  totalRecordCount: number = 0;
  private _authorService = inject(AuthorService);
  private _helperService = inject(HelperService);
  private _storageService = inject(StorageService);
  private _snackBar = inject(MatSnackBar);
  private _router = inject(Router);
  private _activatedRoute = inject(ActivatedRoute);
  readonly dialog = inject(MatDialog);
  displayedColumns = ['authorName', 'bookCount', 'action'];
  pageIndex: number = 0;
  pageSize: number = 10;
  sortField: string = 'AuthorName';
  sortDirection: SortDirection = 'asc';
  searchText: string = '';
  constructor() {
    this.searchText = this._activatedRoute.snapshot.queryParamMap.get('searchText') || '';
  }
  ngOnInit(): void {
    this.getAuthorList();
  }

  ngAfterViewInit(): void {
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
      this.paginator.page.subscribe((event: PageEvent) => {
        this.pageIndex = event.pageIndex;
        this.pageSize = event.pageSize;
        this.getAuthorList();
      });
    }
    if (this.sort) {
      this.dataSource.sort = this.sort;
      this.sort.sortChange.subscribe((sort: Sort) => {
        this.sortField = sort.active;
        this.sortDirection = sort.direction;
        this.getAuthorList();
      });
    }
  }


  getAuthorList() {
    this._authorService.authorList(this.pageIndex, this.pageSize, this.searchText, this._helperService.capitalizeFirstLetter(this.sortField), this.sortDirection === 'asc' ? 0 : 1)
      .subscribe({
        next: (data: any) => {
          if (data?.isSuccess) {
            this.totalRecordCount = data.totalRecordCount;
            this.dataSource = new MatTableDataSource(data.records);
          }
          else {
            this.totalRecordCount = 0;
            this.dataSource = new MatTableDataSource();
          }
        },
        error: (err: any) => {
          this._snackBar.open('There was an issue while fetching the author list. Please try again later.', 'Close', {
            duration: 5000
          })
        }
      })
  }

  applyFilter(_searchText: string) {
    this.searchText = _searchText;
    this._helperService.navigateWithSearchParams({ searchText: this.searchText });
    this.getAuthorList();
  }

  deleteAuthor(id: number) {
    let data: ConfirmDialogData = {
      title: 'Delete Author?',
      message: 'Are you sure you want to delete this author?',
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
        this._authorService.deleteAuthor(id).subscribe({
          next: () => {
            this.getAuthorList();
            this._snackBar.open('Author successfully deleted.', 'Close', {
              duration: 5000
            })
          },
          error: (err) => {
            this._snackBar.open('There was an issue deleting the author. Please try again later.', 'Close', {
              duration: 5000
            })
          }
        });
      }
    });

  }

  editAuthor(id: number) {
    const dialogRef = this.dialog.open(AuthorAddEditComponent, {
      data: { id: id },
      height: '400px',
      width: '400px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.getAuthorList();
      }
    });
  }

  openAddAuthor() {
    const dialogRef = this.dialog.open(AuthorAddEditComponent, {
      height: '400px',
      width: '400px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.getAuthorList();
      }
    });
  }

  goToBookList(id: number) {
    this._router.navigate(["/books"], { queryParams: { authorId: id } });
  }

  get currentUser(): any {
    return this._storageService.getLoggedInUser();
  }
}
