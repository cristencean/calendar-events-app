import { CalendarEvent } from '../models/event.model';

export interface CalendarState {
  events: CalendarEvent[];
  loading: boolean;
  error: string | null;
}

export interface AppState {
  calendar: CalendarState;
}