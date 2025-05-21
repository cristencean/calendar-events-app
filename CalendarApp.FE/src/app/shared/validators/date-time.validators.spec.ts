import { FormGroup, FormControl } from '@angular/forms';
import { endDateAfterStartDateValidator } from './date-time.validators';

describe('endDateAfterStartDateValidator', () => {
  function createForm(startDate: string, endDate: string): FormGroup {
    return new FormGroup({
      startDate: new FormControl(startDate),
      endDate: new FormControl(endDate),
    }, { validators: [endDateAfterStartDateValidator()] });
  }

  it('should return null if endDate is after startDate', () => {
    const form = createForm('2025-01-01T10:00:00', '2025-01-01T12:00:00');
    expect(form.errors).toBeNull();
  });

  it('should return error if endDate is before startDate', () => {
    const form = createForm('2025-01-01T12:00:00', '2025-01-01T10:00:00');
    expect(form.errors).toEqual({ endDateBeforeStartDate: true });
  });

  it('should return error if endDate is equal to startDate', () => {
    const form = createForm('2025-01-01T10:00:00', '2025-01-01T10:00:00');
    expect(form.errors).toEqual({ endDateBeforeStartDate: true });
  });

  it('should return null if either startDate or endDate is missing', () => {
    const form1 = createForm('', '2025-01-01T10:00:00');
    const form2 = createForm('2025-01-01T10:00:00', '');

    expect(form1.errors).toBeNull();
    expect(form2.errors).toBeNull();
  });
});
