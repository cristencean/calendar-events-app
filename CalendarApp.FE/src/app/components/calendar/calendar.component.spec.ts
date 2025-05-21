import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CalendarComponent } from './calendar.component';
import { provideMockStore, MockStore } from '@ngrx/store/testing';
import { MatDialog } from '@angular/material/dialog';
import { of } from 'rxjs';
import * as calendarActions from '../../store/calendar.actions';
import { CalendarEvent } from '../../models/event.model';import {
    CalendarEvent as AngularCalendarEvent,
} from 'angular-calendar';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('CalendarComponent', () => {
  let component: CalendarComponent;
  let fixture: ComponentFixture<CalendarComponent>;
  let store: MockStore;
  let dialogSpy: jasmine.SpyObj<MatDialog>;

  const mockEvents: CalendarEvent[] = [
    {
      id: '1',
      title: 'Event One',
      description: 'Test',
      startDate: '2025-05-20T10:00:00',
      endDate: '2025-05-20T11:00:00'
    }
  ];

  const initialState = {
    calendar: {
      events: mockEvents,
      loading: false,
      error: null
    }
  };

  beforeEach(async () => {
    dialogSpy = jasmine.createSpyObj('MatDialog', ['open']);

    await TestBed.configureTestingModule({
      declarations: [CalendarComponent],
      providers: [
        provideMockStore({ initialState }),
        { provide: MatDialog, useValue: dialogSpy }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    }).compileComponents();

    store = TestBed.inject(MockStore);
    fixture = TestBed.createComponent(CalendarComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should dispatch loadEvents on init', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    component.ngOnInit();
    expect(dispatchSpy).toHaveBeenCalledWith(calendarActions.loadEvents());
  });

  it('should map calendar events to AngularCalendarEvent format', (done) => {
    component.ngOnInit();
    component.events$.subscribe((events: AngularCalendarEvent[]) => {
      expect(events.length).toBe(1);
      expect(events[0].title).toBe('Event One');
      expect(events[0].meta.original.id).toBe('1');
      done();
    });
  });

  it('should dispatch addEvent when dialog returns new event', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    const result = { ...mockEvents[0], type: 'add' };
    dialogSpy.open.and.returnValue({ afterClosed: () => of(result) } as any);

    component.addEvent();

    expect(dispatchSpy).toHaveBeenCalledWith(
      calendarActions.addEvent({ event: result })
    );
  });

  it('should dispatch updateEvent when dialog returns type update', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    const event = mockEvents[0];
    const dialogResult = { ...event, type: 'update' };

    dialogSpy.open.and.returnValue({ afterClosed: () => of(dialogResult) } as any);

    component.handleEventClick({
      start: new Date(event.startDate),
      end: new Date(event.endDate),
      title: event.title,
      meta: { original: event }
    });

    expect(dispatchSpy).toHaveBeenCalledWith(
      calendarActions.updateEvent({ event: dialogResult })
    );
  });

  it('should dispatch deleteEvent when dialog returns type delete', () => {
    const dispatchSpy = spyOn(store, 'dispatch');
    const event = mockEvents[0];
    const dialogResult = { ...event, type: 'delete' };

    dialogSpy.open.and.returnValue({ afterClosed: () => of(dialogResult) } as any);

    component.handleEventClick({
      start: new Date(event.startDate),
      end: new Date(event.endDate),
      title: event.title,
      meta: { original: event }
    });

    expect(dispatchSpy).toHaveBeenCalledWith(
      calendarActions.deleteEvent({ eventId: event.id })
    );
  });
});
