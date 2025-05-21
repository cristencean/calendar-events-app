import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { CalendarView } from 'angular-calendar';
import { map } from 'rxjs/operators';
import { CalendarEvent as AngularCalendarEvent } from 'angular-calendar';
import { AppState } from '../../store/app.state';
import * as calendarActions from '../../store/calendar.actions';
import { EventDialogComponent } from '../event-dialog/event-dialog.component';
import { CalendarEvent } from '../../models/event.model';

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CalendarComponent implements OnInit {
  viewDate: Date = new Date();
  view: CalendarView = CalendarView.Month;
  CalendarView = CalendarView;
  
  events$!: Observable<AngularCalendarEvent[]>;

  constructor(private store: Store<AppState>, private dialog: MatDialog) {}

  ngOnInit(): void {
    this.store.dispatch(calendarActions.loadEvents());
    this.loadEvents();
  }

  private loadEvents(): void {
    this.events$ = this.store.select(state => state.calendar.events).pipe(
      map(this.mapEventsToAngularCalendarEvents)
    );
  }

  private mapEventsToAngularCalendarEvents(events: CalendarEvent[]): AngularCalendarEvent[] {
    return events.map(event => ({
      start: new Date(event.startDate),
      end: event.endDate ? new Date(event.endDate) : undefined,
      title: event.title,
      meta: {
        id: event.id,
        description: event.description,
        original: event,
      },
    }));
  }

  handleEventClick(event: AngularCalendarEvent): void {
    const originalEvent = event.meta.original;
    this.openEventDialog(originalEvent).afterClosed().subscribe(result => {
      if (result) {
        this.handleDialogResult(result, originalEvent);
      }
    });
  }

  addEvent(): void {
    this.openEventDialog().afterClosed().subscribe(result => {
      if (result) {
        this.store.dispatch(calendarActions.addEvent({ event: result }));
      }
    });
  }

  private openEventDialog(event?: CalendarEvent) {
    return this.dialog.open(EventDialogComponent, {
      data: event,
    });
  }

  private handleDialogResult(result: any, originalEvent: CalendarEvent): void {
    if (result.type === 'update') {
      this.store.dispatch(calendarActions.updateEvent({ event: result }));
    } else if (result.type === 'delete') {
      this.store.dispatch(calendarActions.deleteEvent({ eventId: originalEvent.id }));
    }
  }
}
