import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChild, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatIcon } from '@angular/material/icon';
import { MatInput } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSort, MatSortModule, Sort, SortDirection } from '@angular/material/sort';
import { MatTable, MatTableDataSource, MatTableModule } from '@angular/material/table';
import { FeaturePageHeaderComponent } from '../../shared/feature-page-header/feature-page-header.component';
import { FormsModule } from '@angular/forms';
import { PublisherListItem } from './publisher-list-item';
import { PublisherService } from '../../../_services/publisher/publisher.service';
import { HelperService } from '../../../_helpers/helper.service';
import { AuthService } from '../../../_services/auth/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { PublisherAddEditComponent } from '../publisher-add-edit/publisher-add-edit.component';
import { ConfirmDialogComponent } from '../../shared/confirm-dialog/confirm-dialog.component';
import { ConfirmDialogData } from '../../shared/confirm-dialog/confirm-dialog-data';
import { Subscription } from 'rxjs';
import { StorageService } from '../../../_services/storage.service';

@Component({
  selector: 'app-publisher-list',
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
  ],
  templateUrl: './publisher-list.component.html',
  styleUrl: './publisher-list.component.scss'
})
export class PublisherListComponent implements AfterViewInit, OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<PublisherListItem>;

  dataSource: MatTableDataSource<any> = new MatTableDataSource();
  totalRecordCount: number = 0;
  private _publisherService = inject(PublisherService);
  private _helperService = inject(HelperService);
  private _storageService = inject(StorageService);
  private _snackBar = inject(MatSnackBar);
  private _router = inject(Router);
  private _route = inject(ActivatedRoute);
  readonly dialog = inject(MatDialog);
  displayedColumns = ['publisherName', 'bookCount', 'action'];
  pageIndex: number = 0;
  pageSize: number = 10;
  sortField: string = 'PublisherName';
  sortDirection: SortDirection = 'asc';
  searchText: string = '';
  constructor() {
    this.searchText = this._route.snapshot.queryParamMap.get('searchText') || '';
  }


  ngAfterViewInit(): void {
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
      this.paginator.page.subscribe((event: PageEvent) => {
        this.pageIndex = event.pageIndex;
        this.pageSize = event.pageSize;
        this.getPublisherList();
      });
    }
    if (this.sort) {
      this.dataSource.sort = this.sort;
      this.sort.sortChange.subscribe((sort: Sort) => {
        this.sortField = sort.active;
        this.sortDirection = sort.direction;
        this.getPublisherList();
      });
    }
  }

  ngOnInit(): void {
    this.getPublisherList();
  }


  getPublisherList() {
    this._publisherService.publisherList(this.pageIndex, this.pageSize, this.searchText, this._helperService.capitalizeFirstLetter(this.sortField), this.sortDirection === 'asc' ? 0 : 1)
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
          this._snackBar.open('There was an issue while fetching the publisher list. Please try again later.', 'Close', {
            duration: 5000
          })
        }
      })
  }

  applyFilter(_searchText: string) {
    this.searchText = _searchText;
    this._helperService.navigateWithSearchParams({ searchText: this.searchText });
    this.getPublisherList();
  }

  deletePublisher(id: number) {
    let data: ConfirmDialogData = {
      title: 'Delete Publisher?',
      message: 'Are you sure you want to delete this publisher?',
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
        this._publisherService.deletePublisher(id).subscribe({
          next: () => {
            this.getPublisherList();
            this._snackBar.open('Publisher successfully deleted.', 'Close', {
              duration: 5000
            })
          },
          error: (err) => {
            this._snackBar.open('There was an issue deleting the publisher. Please try again later.', 'Close', {
              duration: 5000
            })
          }
        });
      }
    });

  }

  editPublisher(id: number) {
    const dialogRef = this.dialog.open(PublisherAddEditComponent, {
      data: { id: id },
      height: '250px',
      width: '400px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.getPublisherList();
      }
    });
  }

  openAddPublisher() {
    const dialogRef = this.dialog.open(PublisherAddEditComponent, {
      height: '250px',
      width: '400px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.getPublisherList();
      }
    });
  }
  goToBookList(id: number) {
    this._router.navigate(["/books"], { queryParams: { publisherId: id } });
  }

  get currentUser(): any {
    return this._storageService.getLoggedInUser();
  }
}
