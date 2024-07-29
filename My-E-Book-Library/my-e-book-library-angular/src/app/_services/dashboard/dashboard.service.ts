import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  httpClient = inject(HttpClient)
  constructor() { }

  authorList(): any {
    return this.httpClient.get('/api/dashboard/authorlist');
  }

  genreList(): any {
    return this.httpClient.get('/api/dashboard/genrelist');
  }
  languageList(): any {
    return this.httpClient.get('/api/dashboard/languagelist');
  }
  publisherList(): any {
    return this.httpClient.get('/api/dashboard/publisherlist');
  }

}
