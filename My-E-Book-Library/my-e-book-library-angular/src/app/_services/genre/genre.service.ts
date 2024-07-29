import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/internal/operators/map';

@Injectable({
  providedIn: 'root'
})
export class GenreService {
  httpClient = inject(HttpClient)

  constructor() { }


  genreList(pageIndex: number, pageSize: number, searchText: string, sortColumn: string, sortOrder: number): any {
    return this.httpClient.get('/api/genres/genrelist/?' + 'pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&sortOrder=' + sortOrder + '&searchText=' + searchText + '&sortColumn=' + sortColumn);
  }

  deleteGenre(id: number) {
    return this.httpClient.delete('/api/genres?genreId=' + id,)
  }

  genreDropdownList(search: string | null): Observable<any[]> {
    return this.httpClient.get<any>(`/api/genres/genredropdownlist?q=${search}`).pipe(
      map(response => response.records)
    );
  }

  saveGenre(data: any) {
    return this.httpClient.post('/api/genres/savegenre', data,)
  }

  getGenre(id: number) {
    return this.httpClient.get('/api/genres?genreId=' + id);
  }

}
