import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild, inject, viewChild } from '@angular/core';
import { MatTableModule, MatTable, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSortModule, MatSort, Sort, SortDirection } from '@angular/material/sort';
import { MatSnackBar } from '@angular/material/snack-bar';
import { GenreListItem } from './genre-list-item';
import { FeaturePageHeaderComponent } from '../../shared/feature-page-header/feature-page-header.component';
import { GenreService } from '../../../_services/genre/genre.service';
import { HelperService } from '../../../_helpers/helper.service';
import { MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatIcon } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { AuthService } from '../../../_services/auth/auth.service';
import { Subscription, catchError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, ActivatedRouteSnapshot, Router } from '@angular/router';
import { StorageService } from '../../../_services/storage.service';
import { GenreAddEditComponent } from '../genre-add-edit/genre-add-edit.component';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialogData } from '../../shared/confirm-dialog/confirm-dialog-data';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';

@Component({
  selector: 'app-genre-list',
  templateUrl: './genre-list.component.html',
  styleUrl: './genre-list.component.scss',
  standalone: true,
  imports: [MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatMenuModule, MatFormField, MatLabel, MatInput, MatSuffix, MatIcon, FeaturePageHeaderComponent, FormsModule]
})
export class GenreListComponent implements AfterViewInit, OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<GenreListItem>;

  dataSource: MatTableDataSource<any> = new MatTableDataSource();
  totalRecordCount: number = 0;
  private _genreService = inject(GenreService);
  private _helperService = inject(HelperService);
  private _storageService = inject(StorageService);
  private _snackBar = inject(MatSnackBar);
  private _route = inject(ActivatedRoute);
  readonly dialog = inject(MatDialog);
  private _router = inject(Router);

  displayedColumns: string[];
  pageIndex: number = 0;
  pageSize: number = 10;
  sortField: string = 'GenreName';
  sortDirection: SortDirection = 'asc';
  searchText: string = '';
  constructor() {
    this.searchText = this._route.snapshot.queryParamMap.get('searchText') || '';
    this.displayedColumns = this.currentUser.role === 'Admin' ? ['genreName', 'bookCount', 'action'] : ['genreName', 'bookCount'];
  }

  ngAfterViewInit(): void {
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
      this.paginator.page.subscribe((event: PageEvent) => {
        this.pageIndex = event.pageIndex;
        this.pageSize = event.pageSize;
        this.getGenreList();
      });
    }
    if (this.sort) {
      this.dataSource.sort = this.sort;
      this.sort.sortChange.subscribe((sort: Sort) => {
        this.sortField = sort.active;
        this.sortDirection = sort.direction;
        this.getGenreList();
      });
    }
  }

  ngOnInit(): void {
    this.getGenreList();
  }


  getGenreList() {
    this._genreService.genreList(this.pageIndex, this.pageSize, this.searchText, this._helperService.capitalizeFirstLetter(this.sortField), this.sortDirection === 'asc' ? 0 : 1)
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
          this._snackBar.open('There was an issue while fetching the genre list. Please try again later.', 'Close', {
            duration: 5000
          })
        }
      })
  }

  applyFilter(_searchText: string) {
    this.searchText = _searchText;
    this._helperService.navigateWithSearchParams({ searchText: this.searchText });
    this.getGenreList();
  }

  deleteGenre(id: number) {
    let data: ConfirmDialogData = {
      title: 'Delete Genre?',
      message: 'Are you sure you want to delete this genre?',
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
        this._genreService.deleteGenre(id).subscribe({
          next: () => {
            this.getGenreList();
            this._snackBar.open('Genre successfully deleted.', 'Close', {
              duration: 5000
            })
          },
          error: (err) => {
            this._snackBar.open('There was an issue deleting the genre. Please try again later.', 'Close', {
              duration: 5000
            })
          }
        });
      }
    });

    this._genreService.deleteGenre(id).subscribe({
      next: () => {
        this.getGenreList();
        this._snackBar.open('Genre successfully deleted.', 'Close', {
          duration: 5000
        })
      },
      error: (err) => console.log(err)
    });
  }

  editGenre(id: number) {
    const dialogRef = this.dialog.open(GenreAddEditComponent, {
      data: { id: id },
      height: '250px',
      width: '400px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.getGenreList();
      }
    });
  }

  openAddGenre() {
    const dialogRef = this.dialog.open(GenreAddEditComponent, {
      height: '250px',
      width: '400px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.getGenreList();
      }
    });
  }
  goToBookList(id: number) {
    this._router.navigate(["/books"], { queryParams: { genreId: id } });
  }


  get currentUser(): any {
    return this._storageService.getLoggedInUser();
  }
}
