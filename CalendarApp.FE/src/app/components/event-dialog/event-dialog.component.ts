import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Store } from '@ngrx/store';
import { CalendarEvent } from '../../models/event.model';
import { endDateAfterStartDateValidator } from '../../shared/validators/date-time.validators';
import { eventOverlapValidator } from '../../shared/validators/event-overlap.validators';
import { selectAllEvents } from '../../store/calendar.selectors';
import { take } from 'rxjs';
import { AppState } from '../../store/app.state';

@Component({
    selector: 'app-event-dialog',
    templateUrl: './event-dialog.component.html',
    styleUrls: ['./event-dialog.component.scss'],
    standalone: false
})
export class EventDialogComponent implements OnInit {
    eventForm!: FormGroup;
    isEditMode: boolean;

    constructor(
        private formBuilder: FormBuilder,
        private store: Store<AppState>,
        private dialogRef: MatDialogRef<EventDialogComponent>,
        @Inject(MAT_DIALOG_DATA) public eventData: CalendarEvent | null
    ) {
        this.isEditMode = Boolean(eventData);
    }

    ngOnInit(): void {
        this.store.select(selectAllEvents).pipe(take(1)).subscribe(allEvents => {
            this.eventForm = this.createEventForm(this.eventData, allEvents);
        });
    }

    private createEventForm(eventData: CalendarEvent | null, allEvents: CalendarEvent[]): FormGroup {
        return this.formBuilder.group({
            id: [eventData?.id ?? null],
            title: [eventData?.title ?? '', [Validators.required, Validators.maxLength(250)]],
            description: [eventData?.description ?? '', Validators.maxLength(2000)],
            startDate: [eventData?.startDate ?? '', [Validators.required]],
            endDate: [eventData?.endDate ?? '', [Validators.required]],
        }, {
            validators: [endDateAfterStartDateValidator(), eventOverlapValidator(allEvents)]
        });
    }

    onSubmit(): void {
        if (this.eventForm.valid) {
            this.dialogRef.close({ ...this.eventForm.value, type: this.isEditMode ? 'update' : 'create' });
        }
    }

    onDelete(): void {
        if (this.eventForm.valid) {
            this.dialogRef.close({ ...this.eventForm.value, type: 'delete' });
        }
    }

    onCancel(): void {
        this.dialogRef.close();
    }
}
