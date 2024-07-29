import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ProfileService {
  httpClient = inject(HttpClient)

  constructor() { }

  profile(): any {
    return this.httpClient.get('/api/profile');
  }

  saveProfile(data: any): any {
    return this.httpClient.post('/api/profile', data);
  }

  changePassword(data: any): any {
    return this.httpClient.post('/api/profile/changepassword', data);
  }
}
