import { AbstractControl, ValidatorFn } from "@angular/forms";

export function passwordsMatchValidator(): ValidatorFn {
    return (formGroup: AbstractControl) => {
        const password = formGroup.get('password');
        const confirmPassword = formGroup.get('confirmPassword');

        if (password && confirmPassword) {
            if (confirmPassword.errors && !confirmPassword.errors['passwordsDoNotMatch']) {
                return null;
            }

            if (password.value && confirmPassword.value && password.value !== confirmPassword.value) {
                confirmPassword.setErrors({ passwordsDoNotMatch: true });
            }
            else {
                if (confirmPassword.errors) {
                    delete confirmPassword.errors['passwordsDoNotMatch'];
                    if (!Object.keys(confirmPassword.errors).length) {
                        confirmPassword.setErrors(null);
                    }
                } else {
                    confirmPassword.setErrors(null);
                }
            }
        }

        return null;
    };
}