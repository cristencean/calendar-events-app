import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { provideNativeDateAdapter } from '@angular/material/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
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

import { environment } from '../environments/environment';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { EventDialogComponent } from './components/event-dialog/event-dialog.component';

import { calendarReducer } from './store/calendar.reducer';
import { CalendarEffects } from './store/calendar.effects';
import { ErrorLabelComponent } from './components/error-label/error-label.component';
import { GLOBAL_DATE_FORMATS } from './shared/helpers/date-formats';

@NgModule({
    declarations: [
        AppComponent,
        CalendarComponent,
        EventDialogComponent,
        ErrorLabelComponent,
    ],
    imports: [
        MatMomentDateModule,
        BrowserModule,
        AppRoutingModule,
        HttpClientModule,
        FormsModule,
        BrowserAnimationsModule,
        MatCardModule,
        MatIconModule,
        MatDialogModule,
        MatButtonModule,
        MatFormFieldModule,
        MatInputModule,
        MatSnackBarModule,
        NgxMatDatepickerActions,
        NgxMatDatepickerApply,
        NgxMatDatepickerCancel,
        NgxMatDatepickerClear,
        NgxMatDatepickerInput,
        NgxMatDatetimepicker,
        ReactiveFormsModule,
        StoreDevtoolsModule.instrument({
            maxAge: 25,
            logOnly: environment.production,
        }),
        StoreModule.forRoot({
            calendar: calendarReducer
        }),
        EffectsModule.forRoot([CalendarEffects]),
        CalendarModule.forRoot({
            provide: DateAdapter,
            useFactory: adapterFactory,
        }),
    ],
    providers: [
        provideNativeDateAdapter(),
        { provide: MatCoreDateAdaptor, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
        { provide: MAT_DATE_FORMATS, useValue: GLOBAL_DATE_FORMATS },
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }