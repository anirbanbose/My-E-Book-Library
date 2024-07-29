import { Injectable, inject, signal } from '@angular/core';
import { HttpClient, HttpHandlerFn, HttpRequest } from '@angular/common/http';
import { StorageService } from '../storage.service';
import { BehaviorSubject, Observable, Subject, catchError, pipe, tap, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  httpClient = inject(HttpClient);
  storageService = inject(StorageService);
  router = inject(Router);

  public saveUser(user: any) {
    if (user) {
      this.storageService.setUserData(user);
    }
  }


  login(loginModel: { email: string, password: string, rememberMe: boolean, deviceId: string, userAgent: string }): any {
    return this.httpClient.post(`api/auth/login`, loginModel)
      .pipe(
        tap((data: any) => {
          if (data && data.isLoggedIn) {
            this.saveUser(data.user);
            this.storageService.setDeviceId(data.deviceId);
            return data.user;
          }
          else {
            return null;
          }
        }),
        catchError(this.handleError)
      );;
  }

  refreshToken(): any {
    if (this.isLoggedIn()) {
      let refreshTokenModel = { accessToken: null, refreshToken: null, email: this.storageService.getLoggedInUser().email, deviceId: this.storageService.getDeviceId() };
      return this.httpClient.post(`api/auth/token`, refreshTokenModel);
    }
  }

  isLoggedIn(): boolean {
    return this.storageService.getLoggedInUser() != undefined
  }

  logout() {
    this.storageService.removeUserData();
    this.router.navigate(['account/login']);
  }

  private handleError(error: any) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(() => errorMessage);
  }
}
