import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {
  httpClient = inject(HttpClient)
  constructor() { }


  languageList(pageIndex: number, pageSize: number, searchText: string, sortColumn: string, sortOrder: number): any {
    return this.httpClient.get('/api/languages/languagelist?' + 'pageNumber=' + pageIndex + '&pageSize=' + pageSize + '&sortOrder=' + sortOrder + '&searchText=' + searchText + '&sortColumn=' + sortColumn);
  }

  languageDropdownList(search: string | null): Observable<any[]> {
    return this.httpClient.get<any>(`/api/languages/languagedropdownlist?q=${search}`).pipe(
      map(response => {
        if (response && response.isSuccess)
          return response.records
      })
    );
  }


  getLanguage(id: number) {
    return this.httpClient.get('/api/languages?languageId=' + id);
  }


}
