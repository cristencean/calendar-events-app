import { FormGroup, FormControl } from '@angular/forms';
import { eventOverlapValidator } from './event-overlap.validators';
import { CalendarEvent } from '../../models/event.model';

describe('eventOverlapValidator', () => {
  const existingEvents: CalendarEvent[] = [
    {
      id: '1',
      title: 'Event 1',
      description: '',
      startDate: '2025-05-20T10:00:00',
      endDate: '2025-05-20T11:00:00',
    },
    {
      id: '2',
      title: 'Event 2',
      description: '',
      startDate: '2025-05-20T13:00:00',
      endDate: '2025-05-20T14:00:00',
    },
  ];

  function createForm(id: string, startDate: string, endDate: string) {
    return new FormGroup({
      id: new FormControl(id),
      startDate: new FormControl(startDate),
      endDate: new FormControl(endDate),
    }, {
      validators: [eventOverlapValidator(existingEvents)],
    });
  }

  it('should return null if event does not overlap', () => {
    const form = createForm('3', '2025-05-20T11:00:00', '2025-05-20T13:00:00');
    expect(form.errors).toBeNull();
  });

  it('should return error if event overlaps with existing one', () => {
    const form = createForm('3', '2025-05-20T10:30:00', '2025-05-20T11:30:00');
    expect(form.errors).toEqual({ eventOverlap: true });
  });

  it('should return null if event is updating itself', () => {
    const form = createForm('1', '2025-05-20T10:00:00', '2025-05-20T11:00:00');
    expect(form.errors).toBeNull();
  });

  it('should return null if startDate or endDate is missing', () => {
    const form1 = createForm('3', '', '2025-05-20T12:00:00');
    const form2 = createForm('3', '2025-05-20T12:00:00', '');

    expect(form1.errors).toBeNull();
    expect(form2.errors).toBeNull();
  });
});
