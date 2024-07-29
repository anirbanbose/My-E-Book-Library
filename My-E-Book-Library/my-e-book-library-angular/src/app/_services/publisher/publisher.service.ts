import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/internal/operators/map';

@Injectable({
  providedIn: 'root'
})
export class PublisherService {
  httpClient = inject(HttpClient)

  constructor() { }


  publisherList(pageIndex: number, pageSize: number, searchText: string, sortColumn: string, sortOrder: number): any {
    return this.httpClient.get('/api/publisher/publisherlist?' + 'pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&sortOrder=' + sortOrder + '&searchText=' + searchText + '&sortColumn=' + sortColumn);
  }

  deletePublisher(id: number) {
    return this.httpClient.delete('/api/publisher?publisherId=' + id,)
  }

  getPublisher(id: number) {
    return this.httpClient.get('/api/publisher?publisherId=' + id);
  }

  savePublisher(data: any) {
    return this.httpClient.post('/api/publisher/savepublisher', data,)
  }


  publisherDropdownList(search: string | null): Observable<any[]> {
    return this.httpClient.get<any>(`/api/publisher/publisherdropdownlist?q=${search}`).pipe(
      map(response => response.records)
    );
  }
}
