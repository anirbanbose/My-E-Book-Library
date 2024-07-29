import { Injectable, inject } from "@angular/core";
import { Router } from "@angular/router";
import { Observable } from "rxjs/internal/Observable";

@Injectable({
    providedIn: 'root'
})
export class HelperService {
    private _router = inject(Router);

    capitalizeFirstLetter(str: string) {
        return str.charAt(0).toUpperCase() + str.slice(1);
    }

    navigateWithSearchParams(queryParams: any) {
        this._router.navigate([], {
            queryParams: queryParams,
            queryParamsHandling: 'merge'
        });
    }

    dateWithoutTimezone = (date: Date) => {
        return date.getFullYear().toString() + '-' + ('0' + (date.getMonth() + 1)).slice(-2) + '-' + ('0' + date.getDate()).slice(-2);
    };

    readFile(file: File): Observable<any> {
        return new Observable<any>(observer => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => {
                const fileDetail = { id: 0, fileName: file.name, fileType: file.type, fileSize: file.size, fileData: (reader.result as string).split(',')[1] }
                observer.next(fileDetail);
                observer.complete();
            };
            reader.onerror = error => observer.error(error);
        });
    }

    getErrorMessages(errorObject: any): string[] {
        let errorMessages: string[] = [];
        let error = errorObject.error;
        let propNames = Object.keys(error);
        for (let propName of propNames) {
            errorMessages.push(error[propName][0]);
        }
        return errorMessages;
    }

} 