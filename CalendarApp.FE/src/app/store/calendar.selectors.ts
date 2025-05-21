import { createFeatureSelector, createSelector } from '@ngrx/store';
import { State } from './calendar.reducer';

export const selectCalendarState = createFeatureSelector<State>('calendar');

export const selectAllEvents = createSelector(
  selectCalendarState,
  (state: State) => state.events
);

export const selectLoading = createSelector(
  selectCalendarState,
  (state: State) => state.loading
);

export const selectError = createSelector(
  selectCalendarState,
  (state: State) => state.error
);