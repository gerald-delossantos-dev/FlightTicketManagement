import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbstractControl, FormBuilder, ReactiveFormsModule, ValidationErrors, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin, finalize } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { FlightApiService } from '../../core/services/flight-api.service';
import { FlightClass, FlightSummary, Passenger } from '../../core/models/flight.models';
import { NotificationService } from '../../shared/services/notification.service';

@Component({
  selector: 'app-passengers-page',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressSpinnerModule,
    MatSelectModule,
    MatTableModule
  ],
  templateUrl: './passengers-page.component.html'
})
export class PassengersPageComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly cdr = inject(ChangeDetectorRef);
  readonly classes: FlightClass[] = ['First', 'Business', 'Economy'];
  readonly displayedColumns = ['seatNumber', 'firstName', 'lastName', 'passengerId', 'class', 'ticketPrice', 'numberOfBags', 'totalBaggageWeight'];
  flightNumber = 0;
  flight?: FlightSummary;
  passengers: Passenger[] = [];
  loading = false;
  saving = false;

  readonly form = this.fb.nonNullable.group({
    firstName: ['', Validators.required],
    lastName: ['', Validators.required],
    passengerId: ['', Validators.required],
    class: ['Economy' as FlightClass, Validators.required],
    ticketPrice: [0, [Validators.required, Validators.min(0)]],
    numberOfBags: [0, [Validators.required, Validators.min(0)]],
    totalBaggageWeight: [0, [Validators.required, Validators.min(0)]]
  }, { validators: this.baggageValidator });

  constructor(
    private readonly route: ActivatedRoute,
    private readonly router: Router,
    private readonly api: FlightApiService,
    private readonly notify: NotificationService
  ) {}

  ngOnInit(): void {
    this.flightNumber = Number(this.route.snapshot.paramMap.get('flightNumber'));
    this.load();
  }

  load(): void {
    this.loading = true;
    forkJoin({
      flight: this.api.getFlight(this.flightNumber),
      passengers: this.api.getPassengers(this.flightNumber)
    })
      .pipe(finalize(() => {
        this.loading = false;
        this.cdr.detectChanges();
      }))
      .subscribe({
        next: result => {
          this.flight = result.flight;
          this.passengers = result.passengers;
          this.cdr.detectChanges();
        },
        error: err => this.notify.error(err.message)
      });
  }

  addPassenger(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving = true;
    this.api.addPassenger(this.flightNumber, this.form.getRawValue())
      .pipe(finalize(() => {
        this.saving = false;
        this.cdr.detectChanges();
      }))
      .subscribe({
        next: response => {
          this.notify.success(`${response.message}. Seat ${response.seatNumber} assigned.`);
          this.form.reset({
            firstName: '',
            lastName: '',
            passengerId: '',
            class: 'Economy',
            ticketPrice: 0,
            numberOfBags: 0,
            totalBaggageWeight: 0
          });
          this.load();
          this.cdr.detectChanges();
        },
        error: err => this.notify.error(err.message)
      });
  }

  back(): void {
    this.router.navigate(['/flights']);
  }

  private baggageValidator(control: AbstractControl): ValidationErrors | null {
    const selectedClass = control.get('class')?.value as FlightClass;
    const bags = Number(control.get('numberOfBags')?.value ?? 0);
    const weight = Number(control.get('totalBaggageWeight')?.value ?? 0);
    const rules: Record<FlightClass, { maxBags: number; maxWeight: number }> = {
      First: { maxBags: 2, maxWeight: 30 },
      Business: { maxBags: 2, maxWeight: 20 },
      Economy: { maxBags: 1, maxWeight: 20 }
    };
    const rule = rules[selectedClass];
    return rule && (bags > rule.maxBags || weight > rule.maxWeight) ? { baggageExceeded: true } : null;
  }
}
