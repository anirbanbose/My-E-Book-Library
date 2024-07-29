import { DataSource } from '@angular/cdk/collections';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { map } from 'rxjs/operators';
import { Observable, of as observableOf, merge } from 'rxjs';

// TODO: Replace this with your own data model type
export interface BookListItem {
  id: number;
  title: string;
  genre: string[];
  publisher: string;
  authors: string[];
  languages: string[];
  noOfCopies: number;
}
