import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ErrorLabelComponent } from './error-label.component';
import { MatSnackBar, MatSnackBarRef, MatSnackBarDismiss } from '@angular/material/snack-bar';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { clearErrors } from '../../store/calendar.actions';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';

describe('ErrorLabelComponent', () => {
  let component: ErrorLabelComponent;
  let fixture: ComponentFixture<ErrorLabelComponent>;
  let mockSnackBar: jasmine.SpyObj<MatSnackBar>;
  let mockStore: jasmine.SpyObj<Store>;

  const snackBarRefMock: jasmine.SpyObj<MatSnackBarRef<any>> = jasmine.createSpyObj('MatSnackBarRef', ['afterDismissed']);

  beforeEach(async () => {
    mockSnackBar = jasmine.createSpyObj('MatSnackBar', ['open']);
    mockStore = jasmine.createSpyObj('Store', ['dispatch']);

    const dismissResult: MatSnackBarDismiss = { dismissedByAction: false };
    snackBarRefMock.afterDismissed.and.returnValue(of(dismissResult));
    mockSnackBar.open.and.returnValue(snackBarRefMock);

    await TestBed.configureTestingModule({
      declarations: [ErrorLabelComponent],
      providers: [
        { provide: MatSnackBar, useValue: mockSnackBar },
        { provide: Store, useValue: mockStore }
      ],
      schemas: [CUSTOM_ELEMENTS_SCHEMA]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ErrorLabelComponent);
    component = fixture.componentInstance;
    component.errorMessage = 'Test error';
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should open a snackbar on init', () => {
    expect(mockSnackBar.open).toHaveBeenCalledWith(
      'An error occured: Test error. Please try again!',
      'Close',
      jasmine.any(Object)
    );
  });

  it('should dispatch clearErrors on snackbar dismissed', () => {
    expect(mockStore.dispatch).toHaveBeenCalledWith(clearErrors());
  });
});
