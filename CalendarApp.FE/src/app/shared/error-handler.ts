import { HttpErrorResponse } from "@angular/common/http";
import { Observable, throwError } from "rxjs";

const GENERIC_ERROR_MESSAGE = 'An unknown error occurred';

export const handleError = (error: HttpErrorResponse): Observable<never> => {
    const errorMessage = error.error?.errors?.generalErrors?.join(', ') || GENERIC_ERROR_MESSAGE;
    console.error(`Error: ${errorMessage}`);
    return throwError(() => new Error(errorMessage));
}