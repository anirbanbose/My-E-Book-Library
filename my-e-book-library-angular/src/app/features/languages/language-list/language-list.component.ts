import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild, inject } from '@angular/core';
import { MatTableModule, MatTable, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSortModule, MatSort, SortDirection, Sort } from '@angular/material/sort';
import { LanguageListItem } from './language-list-datasource';
import { FeaturePageHeaderComponent } from '../../shared/feature-page-header/feature-page-header.component';
import { LanguageService } from '../../../_services/language/language.service';
import { HelperService } from '../../../_helpers/helper.service';
import { AuthService } from '../../../_services/auth/auth.service';
import { MatButtonModule } from '@angular/material/button';
import { MatMenuModule } from '@angular/material/menu';
import { MatFormField, MatLabel, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatIcon } from '@angular/material/icon';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs/internal/Subscription';
import { StorageService } from '../../../_services/storage.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-language-list',
  templateUrl: './language-list.component.html',
  styleUrl: './language-list.component.scss',
  standalone: true,
  imports: [MatTableModule, MatPaginatorModule, MatSortModule, MatButtonModule, MatMenuModule, MatFormField, MatLabel, MatInput, MatSuffix, MatIcon, FeaturePageHeaderComponent, FormsModule]
})
export class LanguageListComponent implements AfterViewInit, OnInit {
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatTable) table!: MatTable<LanguageListItem>;
  dataSource: MatTableDataSource<any> = new MatTableDataSource();
  totalRecordCount: number = 0;
  private _languageService = inject(LanguageService);
  private _helperService = inject(HelperService);
  private _storageService = inject(StorageService);
  private _route = inject(ActivatedRoute);
  private _snackBar = inject(MatSnackBar);
  private _router = inject(Router);
  searchText: string = '';
  displayedColumns: string[];
  pageIndex: number = 0;
  pageSize: number = 10;
  sortField: string = 'LanguageName';
  sortDirection: SortDirection = 'asc';

  constructor() {
    this.searchText = this._route.snapshot.queryParamMap.get('searchText') || '';
    this.displayedColumns = this.currentUser.role === 'Admin' ? ['languageName', 'languageCode', 'bookCount', 'action'] : ['languageName', 'languageCode', 'bookCount'];
  }

  ngAfterViewInit(): void {
    if (this.paginator) {
      this.dataSource.paginator = this.paginator;
      this.paginator.page.subscribe((event: PageEvent) => {
        this.pageIndex = event.pageIndex;
        this.pageSize = event.pageSize;
        this.getLanguageList();
      });
    }
    if (this.sort) {
      this.dataSource.sort = this.sort;
      this.sort.sortChange.subscribe((sort: Sort) => {
        this.sortField = sort.active;
        this.sortDirection = sort.direction;
        this.getLanguageList();
      });
    }
  }

  ngOnInit(): void {
    this.getLanguageList();
  }

  getLanguageList() {
    this._languageService.languageList(this.pageIndex, this.pageSize, this.searchText, this._helperService.capitalizeFirstLetter(this.sortField), this.sortDirection === 'asc' ? 0 : 1)
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
          this._snackBar.open('There was an issue while fetching the language list. Please try again later.', 'Close', {
            duration: 5000
          })
        }
      })
  }
  applyFilter(_searchText: string) {
    this.searchText = _searchText;
    this._helperService.navigateWithSearchParams({ searchText: this.searchText });
    this.getLanguageList();
  }

  goToBookList(id: number) {
    this._router.navigate(["/books"], { queryParams: { languageId: id } });
  }

  get currentUser(): any {
    return this._storageService.getLoggedInUser();
  }
}
