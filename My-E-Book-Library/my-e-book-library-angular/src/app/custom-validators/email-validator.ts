import { AbstractControl, AsyncValidatorFn, ValidationErrors } from "@angular/forms";
import { AccountService } from "../_services/account/account.service";
import { Observable, map } from "rxjs";

export class EmailValidator {
    static createValidator(_accountService: AccountService): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors> => {
            return _accountService
                .emailavailable(control.value)
                .pipe(
                    map((result: boolean) => {
                        if (result) {
                            return null
                        }
                        return { emailExists: true };
                    }

                    )
                );
        };
    }
}