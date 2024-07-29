import { Component, OnInit, inject } from '@angular/core';
import { AsyncPipe, CommonModule } from '@angular/common';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { DashboardService } from '../../_services/dashboard/dashboard.service';
import { HttpErrorResponse } from '@angular/common/http';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router, RouterLink } from '@angular/router';

enum SearchType {
  Author = 1,
  Publisher = 2,
  Genre = 3,
  Language = 4
}

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss',
  standalone: true,
  imports: [
    CommonModule,
    AsyncPipe,
    MatGridListModule,
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatTableModule,
    RouterLink
  ]
})
export class DashboardComponent implements OnInit {
  private _snackBar = inject(MatSnackBar);
  private _router = inject(Router);

  authorDataSource: MatTableDataSource<any> = new MatTableDataSource();
  authorColumns = ['authorName', 'bookCount'];

  genreDataSource: MatTableDataSource<any> = new MatTableDataSource();
  genreColumns = ['genreName', 'bookCount'];

  languageDataSource: MatTableDataSource<any> = new MatTableDataSource();
  languageColumns = ['languageName', 'bookCount'];

  publisherDataSource: MatTableDataSource<any> = new MatTableDataSource();
  publisherColumns = ['publisherName', 'bookCount'];

  private _dashboardService = inject(DashboardService);

  constructor() {

  }

  ngOnInit(): void {
    this.getAuthorList();
    this.getGenreList();
    this.getLanguageList();
    this.getPublisherList();
  }

  getAuthorList() {
    this._dashboardService.authorList().subscribe(
      (result: any) => {
        if (result.isSuccess) {
          this.authorDataSource = new MatTableDataSource(result.records);
        }

      },
      (err: HttpErrorResponse) => {
        this._snackBar.open('There was an issue while fetching the author list. Please try again later.', 'Close', {
          duration: 5000
        })
      }
    );
  }

  getGenreList() {
    this._dashboardService.genreList().subscribe(
      (result: any) => {
        if (result.isSuccess) {
          this.genreDataSource = new MatTableDataSource(result.records);
        }

      },
      (err: HttpErrorResponse) => {
        this._snackBar.open('There was an issue while fetching the genre list. Please try again later.', 'Close', {
          duration: 5000
        })
      }
    );
  }

  getLanguageList() {
    this._dashboardService.languageList().subscribe(
      (result: any) => {
        if (result.isSuccess) {
          this.languageDataSource = new MatTableDataSource(result.records);
        }

      },
      (err: HttpErrorResponse) => {
        this._snackBar.open('There was an issue while fetching the language list. Please try again later.', 'Close', {
          duration: 5000
        })
      }
    );
  }

  getPublisherList() {
    this._dashboardService.publisherList().subscribe(
      (result: any) => {
        if (result.isSuccess) {
          this.publisherDataSource = new MatTableDataSource(result.records);
        }

      },
      (err: HttpErrorResponse) => {
        this._snackBar.open('There was an issue while fetching the publisher list. Please try again later.', 'Close', {
          duration: 5000
        })
      }
    );
  }


  goToBookList(id: number, searchType: SearchType) {
    let queryParam = {};
    switch (searchType) {
      case SearchType.Author:
        queryParam = { authorId: id };
        break;
      case SearchType.Publisher:
        queryParam = { publisherId: id };
        break;
      case SearchType.Genre:
        queryParam = { genreId: id };
        break;
      case SearchType.Language:
        queryParam = { languageId: id };
        break;
    }
    this._router.navigate(["/books"], { queryParams: queryParam });
  }
}
