import { Injectable } from '@angular/core';
import { Actions, ofType } from '@ngrx/effects';
import { CalendarService } from '../services/calendar.service';
import * as CalendarActions from './calendar.actions';
import { createEffect } from '@ngrx/effects';
import { catchError, map, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable()
export class CalendarEffects {
    constructor(
        private actions$: Actions,
        private calendarService: CalendarService
    ) { }

    loadEvents$ = createEffect(() =>
        this.actions$.pipe(
            ofType(CalendarActions.loadEvents),
            switchMap(() =>
                this.calendarService.getEvents().pipe(
                    map((events) => CalendarActions.loadEventsSuccess({ events })),
                    catchError((error) => 
                        of(CalendarActions.loadEventsFailure({ error }))
                    )
                )
            )
        )
    );

    addEvent$ = createEffect(() =>
        this.actions$.pipe(
            ofType(CalendarActions.addEvent),
            switchMap(({ event }) =>
                this.calendarService.addEvent(event).pipe(
                    map((newEvent) => CalendarActions.addEventSuccess({ event: newEvent })),
                    catchError((error) => 
                        of(CalendarActions.addEventFailure({ error: error.toString() }))
                    )
                )
            )
        )
    );

    updateEvent$ = createEffect(() =>
        this.actions$.pipe(
            ofType(CalendarActions.updateEvent),
            switchMap(({ event }) =>
                this.calendarService.updateEvent(event).pipe(
                    map(() => CalendarActions.updateEventSuccess({ event })),
                    catchError((error) =>
                        of(CalendarActions.updateEventFailure({ error }))
                    )
                )
            )
        )
    );

    deleteEvent$ = createEffect(() =>
        this.actions$.pipe(
            ofType(CalendarActions.deleteEvent),
            switchMap(({ eventId }) =>
                this.calendarService.deleteEvent(eventId).pipe(
                    map(() => CalendarActions.deleteEventSuccess({ eventId })),
                    catchError((error) =>
                        of(CalendarActions.loadEventsFailure({ error }))
                    )
                )
            )
        )
    );
}