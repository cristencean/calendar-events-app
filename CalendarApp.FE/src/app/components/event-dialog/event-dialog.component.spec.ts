import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EventDialogComponent } from './event-dialog.component';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { CalendarEvent } from '../../models/event.model';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { provideNativeDateAdapter } from '@angular/material/core';
import { MatMomentDateModule, MomentDateAdapter } from '@angular/material-moment-adapter';
import { MAT_DATE_FORMATS, DateAdapter as MatCoreDateAdaptor, MAT_DATE_LOCALE } from '@angular/material/core';
import {
    NgxMatDatepickerActions,
    NgxMatDatepickerApply,
    NgxMatDatepickerCancel,
    NgxMatDatepickerClear,
    NgxMatDatepickerInput,
    NgxMatDatetimepicker,
} from '@ngxmc/datetime-picker';
import { GLOBAL_DATE_FORMATS } from '../../shared/helpers/date-formats';

describe('EventDialogComponent', () => {
  let component: EventDialogComponent;
  let fixture: ComponentFixture<EventDialogComponent>;
  let mockStore: jasmine.SpyObj<Store>;
  let mockDialogRef: jasmine.SpyObj<MatDialogRef<EventDialogComponent>>;

  const mockEvents: CalendarEvent[] = [
    {
      id: '1',
      title: 'Test Event',
      description: 'Details',
      startDate: '2025-05-20T10:00:00',
      endDate: '2025-05-20T12:00:00'
    }
  ];

  const mockDialogData: CalendarEvent = {
    id: '1',
    title: 'Existing Event',
    description: 'Some description',
    startDate: '2025-05-22T14:00:00',
    endDate: '2025-05-22T15:00:00'
  };

  beforeEach(async () => {
    mockStore = jasmine.createSpyObj('Store', ['select']);
    mockDialogRef = jasmine.createSpyObj('MatDialogRef', ['close']);
    mockStore.select.and.returnValue(of(mockEvents));
  
    await TestBed.configureTestingModule({
      declarations: [EventDialogComponent],
      imports: [
        MatMomentDateModule,
        ReactiveFormsModule,
        NoopAnimationsModule,
        MatDialogModule,
        MatFormFieldModule,
        MatInputModule,
        NgxMatDatepickerActions,
        NgxMatDatepickerApply,
        NgxMatDatepickerCancel,
        NgxMatDatepickerClear,
        NgxMatDatepickerInput,
        NgxMatDatetimepicker,
      ],
      providers: [
        FormBuilder,
        provideNativeDateAdapter(),
        { provide: Store, useValue: mockStore },
        { provide: MatDialogRef, useValue: mockDialogRef },
        { provide: MAT_DIALOG_DATA, useValue: mockDialogData },
        { provide: MatCoreDateAdaptor, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
        { provide: MAT_DATE_FORMATS, useValue: GLOBAL_DATE_FORMATS },
      ]
    }).compileComponents();
  });
  

  beforeEach(() => {
    fixture = TestBed.createComponent(EventDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with injected event data', () => {
    expect(component.eventForm).toBeDefined();
    expect(component.eventForm.value.title).toBe(mockDialogData.title);
  });

  it('should validate form and close dialog on submit', () => {
    const mockEvent = {
        id: '1',
        title: 'New Title',
        description: 'Updated description',
        startDate: '2025-05-23T10:00:00',
        endDate: '2025-05-23T11:00:00'
      };
    component.eventForm.setValue(mockEvent);

    component.onSubmit();

    expect(mockDialogRef.close).toHaveBeenCalledWith({
      ...mockEvent,
      type: 'update'
    });
  });

  it('should close the dialog on cancel', () => {
    component.onCancel();
    expect(mockDialogRef.close).toHaveBeenCalled();
  });

  it('should close the dialog with delete type on delete', () => {
    component.onDelete();
    expect(mockDialogRef.close).toHaveBeenCalledWith(jasmine.objectContaining({ type: 'delete' }));
  });
});
