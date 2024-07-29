import { Injectable, inject } from '@angular/core';
import { HttpEvent, HttpRequest, HttpErrorResponse, HttpHandlerFn } from '@angular/common/http';
import { Observable, catchError, switchMap, throwError } from 'rxjs';

import { HttpInterceptorFn } from '@angular/common/http';
import { AuthService } from '../_services/auth/auth.service';


export const HttpRequestInterceptor: HttpInterceptorFn = (req: HttpRequest<any>, next: HttpHandlerFn): Observable<HttpEvent<any>> => {
  const authService = inject(AuthService);

  req = req.clone({
    withCredentials: true
  });

  return next(req).pipe(
    catchError((error: HttpErrorResponse): Observable<HttpEvent<any>> => {
      console.log('HTTP Error logged');
      if (error.status === 401 && !req.url.includes('auth/login')) {
        console.log('Not login');
        if (req.url.includes('auth/token')) {
          console.log('token url');
          authService.logout();
          console.log(error);
          return throwError(() => error);
        }
        else {
          console.log('refresh token');
          return authService.refreshToken().pipe(
            switchMap((): Observable<HttpEvent<any>> => {
              const newReq = req.clone({
                withCredentials: true
              });
              return next(newReq);
            }),
            catchError((innerErr) => {
              console.log(innerErr);
              console.log('refresh token error');
              authService.logout();
              return throwError(() => innerErr);
            })
          );
        }
      }
      return throwError(() => error);
    })
  );
};

