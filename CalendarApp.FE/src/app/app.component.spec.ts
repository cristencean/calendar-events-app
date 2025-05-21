import { ComponentFixture, TestBed } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { Store } from '@ngrx/store';
import { of } from 'rxjs';
import { AppState } from './store/app.state';

describe('AppComponent', () => {
  let component: AppComponent;
  let fixture: ComponentFixture<AppComponent>;
  let mockStore: jasmine.SpyObj<Store<AppState>>;

  beforeEach(async () => {
    mockStore = jasmine.createSpyObj('Store', ['select']);

    await TestBed.configureTestingModule({
      declarations: [AppComponent],
      schemas: [CUSTOM_ELEMENTS_SCHEMA],
      providers: [
        { provide: Store, useValue: mockStore }
      ]
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AppComponent);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should have title "Calendar events application"', () => {
    expect(component.title).toBe('Calendar events application');
  });

  it('should render title in template', () => {
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('Calendar events application');
  });

  it('should set errorMessage$ observable on init', () => {
    const errorMessage = 'Test error';
    mockStore.select.and.returnValue(of(errorMessage));

    fixture.detectChanges();

    component.errorMessage$.subscribe((message) => {
      expect(message).toBe(errorMessage);
    });
  });
});
