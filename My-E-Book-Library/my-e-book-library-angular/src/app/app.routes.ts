import { Routes } from '@angular/router';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { authGuard } from './_helpers/auth-guard/auth.guard';

export const routes: Routes = [
    {
        path: "account",
        loadComponent: () => import('./account/account.component').then((m) => m.AccountComponent),
        children: [
            {
                path: "",
                redirectTo: "login",
                pathMatch: "full"
            },
            {
                path: "login",
                loadComponent: () => import('./account/login/login.component').then((m) => m.LoginComponent),
            },
            {
                path: "register",
                loadComponent: () => import('./account/registration/registration.component').then((m) => m.RegistrationComponent),
            },
            {
                path: "forgot-password",
                loadComponent: () => import('./account/forgot-password/forgot-password.component').then((m) => m.ForgotPasswordComponent),
            },
        ]
    },
    {
        path: "",
        loadComponent: () => import('./features/features.component').then((m) => m.FeaturesComponent),
        canActivate: [authGuard],
        children: [
            {
                path: "",
                redirectTo: "dashboard",
                pathMatch: "full"
            },
            {
                path: "dashboard",
                loadComponent: () => import('./features/dashboard/dashboard.component').then((m) => m.DashboardComponent),
            },
            {
                path: "books",
                loadComponent: () => import('./features/books/book-list/book-list.component').then((m) => m.BookListComponent),
            },
            {
                path: "add-book",
                loadComponent: () => import('./features/books/book-add-edit/book-add-edit.component').then((m) => m.BookAddEditComponent),
            },
            {
                path: "edit-book/:id",
                loadComponent: () => import('./features/books/book-add-edit/book-add-edit.component').then((m) => m.BookAddEditComponent),
            },
            {
                path: "book-detail/:id",
                loadComponent: () => import('./features/books/book-detail/book-detail.component').then((m) => m.BookDetailComponent),
            },
            {
                path: "authors",
                loadComponent: () => import('./features/authors/author-list/author-list.component').then((m) => m.AuthorListComponent),
            },
            {
                path: "publishers",
                loadComponent: () => import('./features/publishers/publisher-list/publisher-list.component').then((m) => m.PublisherListComponent),
            },


        ]
    },
    {
        path: "settings",
        loadComponent: () => import('./features/features.component').then((m) => m.FeaturesComponent),
        canActivate: [authGuard],
        children: [
            {
                path: "languages",
                loadComponent: () => import('./features/languages/language-list/language-list.component').then((m) => m.LanguageListComponent),
            },
            {
                path: "genres",
                loadComponent: () => import('./features/genres/genre-list/genre-list.component').then((m) => m.GenreListComponent),
            },
            {
                path: "profile",
                loadComponent: () => import('./features/settings/profile/profile.component').then((m) => m.ProfileComponent),
            }
        ]
    },
    { path: '**', loadComponent: () => import('./page-not-found/page-not-found.component').then((m) => m.PageNotFoundComponent), }

];
