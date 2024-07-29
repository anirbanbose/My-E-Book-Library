import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/internal/operators/map';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {
  httpClient = inject(HttpClient)

  constructor() { }


  authorList(pageIndex: number, pageSize: number, searchText: string, sortColumn: string, sortOrder: number): any {
    return this.httpClient.get('/api/author/authorlist?' + 'pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&sortOrder=' + sortOrder + '&searchText=' + searchText + '&sortColumn=' + sortColumn);
  }

  deleteAuthor(id: number) {
    return this.httpClient.delete('/api/author?authorId=' + id,)
  }

  getAuthor(id: number) {
    return this.httpClient.get('/api/author?authorId=' + id);
  }

  saveAuthor(data: any) {
    return this.httpClient.post('/api/author/saveauthor', data,)
  }

  authorDropdownList(search: string | null): Observable<any[]> {
    return this.httpClient.get<any>(`/api/author/authordropdownlist?q=${search}`).pipe(
      map(response => {
        if (response && response.isSuccess)
          return response.records
      })
    );
  }
}
