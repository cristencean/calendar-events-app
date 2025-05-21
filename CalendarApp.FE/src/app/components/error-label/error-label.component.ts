import { Component, Input, OnInit } from '@angular/core';

import { MatSnackBar } from '@angular/material/snack-bar';
import { AppState } from '../../store/app.state';
import { Store } from '@ngrx/store';
import { clearErrors } from '../../store/calendar.actions';

@Component({
  selector: 'error-label',
  template: '',
  styles: '',
  standalone: false
})
export class ErrorLabelComponent implements OnInit {
  @Input() errorMessage: string | null = null;

  constructor(private snackBar: MatSnackBar, private store: Store<AppState>) { }

  ngOnInit(): void {
    const snackRef = this.snackBar.open(
      `An error occured: ${this.errorMessage}. Please try again!` || '',
      'Close',
      {
        duration: 7000,
        horizontalPosition: 'center',
        verticalPosition: 'top',
        panelClass: ['error-snackbar-label']
      });

    snackRef.afterDismissed().subscribe(() => {
      this.store.dispatch(clearErrors());
    });
  }
}