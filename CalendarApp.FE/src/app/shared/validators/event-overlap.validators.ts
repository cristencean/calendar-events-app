import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { CalendarEvent } from '../../models/event.model';

function isOverlapping(startA: Date, endA: Date, startB: Date, endB: Date): boolean {
  return startA < endB && endA > startB;
}

export function eventOverlapValidator(existingEvents: CalendarEvent[]): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const currentId = control.get('id')?.value;
    const startDate = control.get('startDate')?.value;
    const endDate = control.get('endDate')?.value;

    if (!startDate || !endDate) {
      return null;
    }

    const start = new Date(startDate);
    const end = new Date(endDate);

    const overlaps = existingEvents.some(event => {
      if (event.id === currentId) 
        return false;

      const eventStart = new Date(`${event.startDate}`);
      const eventEnd = new Date(`${event.endDate}`);
      return isOverlapping(start, end, eventStart, eventEnd);
    });

    return overlaps ? { eventOverlap: true } : null;
  };
}
