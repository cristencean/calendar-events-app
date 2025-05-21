import { handleError } from './error-handler';
import { HttpErrorResponse } from '@angular/common/http';

describe('handleError', () => {
    let consoleErrorSpy: jasmine.Spy;

    beforeEach(() => {
        consoleErrorSpy = spyOn(console, 'error');
    });

    it('should log a specific error message when available in error response', () => {
        const mockErrorResponse: HttpErrorResponse = {
            error: { errors: { generalErrors: ['Id is required'] } },
            status: 400,
            statusText: 'Bad Request',
            url: '/api/calendar-events',
        } as HttpErrorResponse;

        handleError(mockErrorResponse).subscribe({
            error: (err) => {
                expect(err.message).toBe('Id is required');
                expect(consoleErrorSpy).toHaveBeenCalledWith('Error: Id is required');
            }
        });
    });

    it('should log a default error message if no specific error is provided', () => {
        const mockErrorResponse: HttpErrorResponse = {
            error: {},
            status: 500,
            statusText: 'Internal Server Error',
            url: '/api/calendar-events',
        } as HttpErrorResponse;

        handleError(mockErrorResponse).subscribe({
            error: (err) => {
                expect(err.message).toBe('An unknown error occurred');
                expect(consoleErrorSpy).toHaveBeenCalledWith('Error: An unknown error occurred');
            }
        });
    });

    it('should log a default error message if generalErrors is empty', () => {
        const mockErrorResponse: HttpErrorResponse = {
            error: { errors: { generalErrors: [] } },
            status: 400,
            statusText: 'Bad Request',
            url: '/api/calendar-events',
        } as HttpErrorResponse;

        handleError(mockErrorResponse).subscribe({
            error: (err) => {
                expect(err.message).toBe('An unknown error occurred');
                expect(consoleErrorSpy).toHaveBeenCalledWith('Error: An unknown error occurred');
            }
        });
    });

    it('should handle error when generalErrors is undefined in the error response', () => {
        const mockErrorResponse: HttpErrorResponse = {
            error: { errors: {} },
            status: 400,
            statusText: 'Bad Request',
            url: '/api/calendar-events',
        } as HttpErrorResponse;

        handleError(mockErrorResponse).subscribe({
            error: (err) => {
                expect(err.message).toBe('An unknown error occurred');
                expect(consoleErrorSpy).toHaveBeenCalledWith('Error: An unknown error occurred');
            }
        });
    });
});
