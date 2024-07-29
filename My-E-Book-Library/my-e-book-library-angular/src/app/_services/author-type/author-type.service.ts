import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { map } from 'rxjs/internal/operators/map';

@Injectable({
  providedIn: 'root'
})
export class AuthorTypeService {
  httpClient = inject(HttpClient)
  constructor() { }

  authorTypeDropdownList(): Observable<any[]> {
    return this.httpClient.get<any>(`/api/authortypes/authortypedropdownlist`);
  }
}
