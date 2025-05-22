import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

// NgRx
import { StoreModule } from '@ngrx/store';
import { EffectsModule } from '@ngrx/effects';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';

// Angular Calendar
import { CalendarModule, DateAdapter } from 'angular-calendar';
import { adapterFactory } from 'angular-calendar/date-adapters/date-fns';

// Material Components
import MaterialModule from './shared/modules/material.module';

// Custom Date Adapter
import { MatMomentDateModule, MomentDateAdapter } from '@angular/material-moment-adapter';
import {
    MAT_DATE_FORMATS,
    DateAdapter as MatCoreDateAdapter,
    MAT_DATE_LOCALE
} from '@angular/material/core';
import { GLOBAL_DATE_FORMATS } from './shared/helpers/date-formats';
import { provideNativeDateAdapter } from '@angular/material/core';

// Environment
import { environment } from '../environments/environment';

// App Modules and Components
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { EventDialogComponent } from './components/event-dialog/event-dialog.component';
import { ErrorLabelComponent } from './components/error-label/error-label.component';

// State
import { calendarReducer } from './store/calendar.reducer';
import { CalendarEffects } from './store/calendar.effects';
import DatePickerModule from './shared/modules/date-picker.module';

@NgModule({
    declarations: [
        AppComponent,
        CalendarComponent,
        EventDialogComponent,
        ErrorLabelComponent,
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        AppRoutingModule,
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule,

        MaterialModule,
        DatePickerModule,
        MatMomentDateModule,

        StoreModule.forRoot({ calendar: calendarReducer }),
        EffectsModule.forRoot([CalendarEffects]),
        StoreDevtoolsModule.instrument({
            maxAge: 25,
            logOnly: environment.production,
        }),

        CalendarModule.forRoot({
            provide: DateAdapter,
            useFactory: adapterFactory,
        }),
    ],
    providers: [
        provideNativeDateAdapter(),
        { provide: MatCoreDateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE] },
        { provide: MAT_DATE_FORMATS, useValue: GLOBAL_DATE_FORMATS },
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
