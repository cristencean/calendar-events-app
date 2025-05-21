import { createReducer, on } from '@ngrx/store';
import * as calendarActions from './calendar.actions';
import { CalendarEvent } from '../models/event.model';

export interface State {
    events: CalendarEvent[];
    loading: boolean;
    error: string | null;
}

export const initialState: State = {
    events: [],
    loading: false,
    error: null,
};

export const calendarReducer = createReducer(
    initialState,

    on(calendarActions.loadEvents, (state) => ({
        ...state,
        loading: true
    })),

    on(calendarActions.loadEventsSuccess, (state, { events }) => ({
        ...state,
        loading: false,
        events
    })),

    on(calendarActions.loadEventsFailure, (state, { error }) => ({
        ...state,
        loading: false,
        error
    })),

    on(calendarActions.addEventSuccess, (state, { event }) => ({
        ...state,
        events: [...state.events, event]
    })),

    on(calendarActions.addEventFailure, (state, { error }) => ({
        ...state,
        loading: false,
        error
    })),

    on(calendarActions.deleteEventSuccess, (state, { eventId }) => ({
        ...state,
        events: state.events.filter(event => event.id !== eventId)
    })),

    on(calendarActions.deleteEventFailure, (state, { error }) => ({
        ...state,
        loading: false,
        error
    })),

    on(calendarActions.updateEventSuccess, (state, args) => ({
        ...state,
        events: state.events.map(existingEvent => {
            return existingEvent.id === args.event.id ? args.event : existingEvent;
        })
    })),

    on(calendarActions.updateEventFailure, (state, { error }) => ({
        ...state,
        loading: false,
        error
    })),

    on(calendarActions.clearErrors, (state) => ({
        ...state,
        error: null
    }))
);