import { TestBed } from '@angular/core/testing';
import { CalendarService } from './calendar.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CalendarEvent } from '../models/event.model';
import { environment } from '../../environments/environment';

describe('CalendarService', () => {
  let service: CalendarService;
  let httpMock: HttpTestingController;

  const mockEvents: CalendarEvent[] = [
    {
      id: '1',
      title: 'Testing Meeting',
      description: 'Discuss the test example',
      startDate: '2025-05-21T10:00:00',
      endDate: '2025-05-21T11:00:00',
    }
  ];

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CalendarService],
    });

    service = TestBed.inject(CalendarService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch events via GET', () => {
    service.getEvents().subscribe(events => {
      expect(events.length).toBe(1);
      expect(events).toEqual(mockEvents);
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/calendar-events`);
    expect(req.request.method).toBe('GET');
    req.flush(mockEvents);
  });

  it('should add an event via POST', () => {
    const newEvent = mockEvents[0];

    service.addEvent(newEvent).subscribe(event => {
      expect(event).toEqual(newEvent);
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/calendar-events`);
    expect(req.request.method).toBe('POST');
    req.flush(newEvent);
  });

  it('should update an event via PUT', () => {
    const updatedEvent = { ...mockEvents[0], title: 'Updated Title' };

    service.updateEvent(updatedEvent).subscribe(event => {
      expect(event).toEqual(updatedEvent);
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/calendar-events/${updatedEvent.id}`);
    expect(req.request.method).toBe('PUT');
    req.flush(updatedEvent);
  });

  it('should delete an event via DELETE', () => {
    const id = '1';

    service.deleteEvent(id).subscribe(response => {
      expect(response).toBeNull();
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/calendar-events/${id}`);
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });
});
