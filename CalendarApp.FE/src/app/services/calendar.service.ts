import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { CalendarEvent } from '../models/event.model';
import { environment } from '../../environments/environment';
import { handleError } from '../shared/error-handler';

@Injectable({
    providedIn: 'root',
})
export class CalendarService {
    private apiUrl = environment.apiUrl;

    constructor(private http: HttpClient) { }

    getEvents(): Observable<CalendarEvent[]> {
        return this.http.get<CalendarEvent[]>(`${this.apiUrl}/calendar-events`).pipe(
            catchError((error) => handleError(error))
        );
    }

    addEvent(event: CalendarEvent): Observable<CalendarEvent> {
        return this.http.post<CalendarEvent>(`${this.apiUrl}/calendar-events`, event).pipe(
            catchError((error) => handleError(error))
        );
    }

    updateEvent(event: CalendarEvent): Observable<CalendarEvent> {
        return this.http.put<CalendarEvent>(`${this.apiUrl}/calendar-events/${event.id}`, event).pipe(
            catchError((error) => handleError(error))
        );
    }

    deleteEvent(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/calendar-events/${id}`).pipe(
            catchError((error) => handleError(error))
        );
    }
}