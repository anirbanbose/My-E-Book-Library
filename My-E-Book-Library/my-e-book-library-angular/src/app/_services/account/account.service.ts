import { HttpClient } from "@angular/common/http";
import { inject } from "@angular/core";

export class AccountService {
  httpClient = inject(HttpClient)

  constructor() { }

  emailavailable(email: string): any {
    return this.httpClient.get('/api/account/emailavailable?email=' + email);
  }
  register(data: any): any {
    return this.httpClient.post('/api/account/register', data);
  }
}
