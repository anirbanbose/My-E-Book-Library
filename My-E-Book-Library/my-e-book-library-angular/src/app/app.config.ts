import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideEnvironmentNgxMask } from 'ngx-mask';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { StorageService } from './_services/storage.service';
import { AuthService } from './_services/auth/auth.service';
import { HttpRequestInterceptor } from './_helpers/http.intercepter';
import { HelperService } from './_helpers/helper.service';
import { AccountService } from './_services/account/account.service';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideClientHydration(),
    provideAnimationsAsync(),
    provideHttpClient(withInterceptors([HttpRequestInterceptor])),
    provideEnvironmentNgxMask(),
    StorageService,
    AuthService,
    HelperService,
    AccountService
  ]
};
