import { ChangeDetectorRef, Component, OnInit, AfterViewInit, ViewChild, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { FlightApiService } from '../../core/services/flight-api.service';
import { FlightSummary } from '../../core/models/flight.models';
import { NotificationService } from '../../shared/services/notification.service';

@Component({
  selector: 'app-flights-page',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatTableModule,
    MatSortModule
  ],
  templateUrl: './flights-page.component.html'
})
export class FlightsPageComponent implements OnInit, AfterViewInit {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);

  @ViewChild(MatSort) sort!: MatSort;

  readonly displayedColumns = ['flightNumber', 'destination', 'totalPassengers', 'availableSeats', 'first', 'business', 'economy'];
  readonly dataSource = new MatTableDataSource<FlightSummary>([]);
  loading = false;
  saving = false;

  readonly form = this.fb.nonNullable.group({
    flightNumber: [0, [Validators.required, Validators.min(1)]],
    destination: ['', [Validators.required]]
  });

  constructor(
    private readonly api: FlightApiService,
    private readonly notify: NotificationService,
    private readonly router: Router
  ) {
    this.dataSource.sortingDataAccessor = (item, property) => {
      if (property === 'first') return item.availableSeatsByClass.first;
      if (property === 'business') return item.availableSeatsByClass.business;
      if (property === 'economy') return item.availableSeatsByClass.economy;
      return (item as unknown as Record<string, string | number>)[property] ?? '';
    };
  }

  ngOnInit(): void {
    this.loadFlights();
  }

  ngAfterViewInit(): void {
    this.dataSource.sort = this.sort;
  }

  loadFlights(): void {
    this.loading = true;
    this.api.getFlights()
      .pipe(finalize(() => {
        this.loading = false;
        this.cdr.detectChanges();
      }))
      .subscribe({
        next: flights => {
          this.dataSource.data = flights;
          this.cdr.detectChanges();
        },
        error: err => this.notify.error(err.message)
      });
  }

  addFlight(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.api.addFlight(this.form.getRawValue())
      .pipe(finalize(() => {
        this.saving = false;
        this.cdr.detectChanges();
      }))
      .subscribe({
        next: flight => {
          this.notify.success(`Flight ${flight.flightNumber} added successfully.`);
          this.form.reset({ flightNumber: 0, destination: '' });
          this.loadFlights();
          this.cdr.detectChanges();
        },
        error: err => this.notify.error(err.message)
      });
  }

  openPassengers(flight: FlightSummary): void {
    this.router.navigate(['/flights', flight.flightNumber, 'passengers']);
  }
}
