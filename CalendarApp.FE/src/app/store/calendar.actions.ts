import { createAction, props } from '@ngrx/store';
import { CalendarEvent } from '../models/event.model';

export const loadEvents = createAction('Calendar event Load');

export const loadEventsSuccess = createAction(
  'Calendar event Load Success',
  props<{ events: CalendarEvent[] }>()
);

export const loadEventsFailure = createAction(
  'Calendar event Load Failure',
  props<{ error: string }>()
);

export const addEvent = createAction(
  'Calendar event Add',
  props<{ event: CalendarEvent }>()
);

export const addEventSuccess = createAction(
  'Calendar event Add Success',
  props<{ event: CalendarEvent }>()
);

export const addEventFailure = createAction(
  'Calendar event Add Failure',
  props<{ error: string }>()
);

export const deleteEvent = createAction(
  'Calendar event Delete',
  props<{ eventId: string }>()
);

export const deleteEventSuccess = createAction(
  'Calendar event Delete Success',
  props<{ eventId: string }>()
);

export const deleteEventFailure = createAction(
  'Calendar event Delete Failure',
  props<{ error: string }>()
);

export const updateEvent = createAction(
  'Calendar event Update',
  props<{ event: CalendarEvent }>()
);

export const updateEventSuccess = createAction(
  'Calendar event Update Success',
  props<{ event: CalendarEvent }>()
);

export const updateEventFailure = createAction(
  'Calendar event Update Failure',
  props<{ error: string }>()
);

export const clearErrors = createAction(
  'Calendar event Clear Error'
);