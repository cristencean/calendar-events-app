import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AppState } from './store/app.state';
import { Store } from '@ngrx/store';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  standalone: false
})
export class AppComponent implements OnInit {
  errorMessage$!: Observable<string | null>;
  title = 'Calendar events application';
  
  constructor(private store: Store<AppState>) {}

  ngOnInit(): void {
    this.setErrorWatcher();
  }

  private setErrorWatcher(): void {
    this.errorMessage$ = this.store.select(state => state.calendar.error);
  }
}
