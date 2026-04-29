import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AddFlightRequest, AddPassengerRequest, AddPassengerResponse, FlightSummary, Passenger } from '../models/flight.models';

@Injectable({ providedIn: 'root' })
export class FlightApiService {
  private readonly baseUrl = environment.apiBaseUrl;

  constructor(private readonly http: HttpClient) {}

  getFlights(): Observable<FlightSummary[]> {
    return this.http.get<FlightSummary[]>(`${this.baseUrl}/flights`).pipe(catchError(this.handleError));
  }

  getFlight(flightNumber: number): Observable<FlightSummary> {
    return this.http.get<FlightSummary>(`${this.baseUrl}/flights/${flightNumber}`).pipe(catchError(this.handleError));
  }

  addFlight(request: AddFlightRequest): Observable<FlightSummary> {
    return this.http.post<FlightSummary>(`${this.baseUrl}/flights`, request).pipe(catchError(this.handleError));
  }

  getPassengers(flightNumber: number): Observable<Passenger[]> {
    return this.http.get<Passenger[]>(`${this.baseUrl}/flights/${flightNumber}/passengers`).pipe(catchError(this.handleError));
  }

  addPassenger(flightNumber: number, request: AddPassengerRequest): Observable<AddPassengerResponse> {
    return this.http.post<AddPassengerResponse>(`${this.baseUrl}/flights/${flightNumber}/passengers`, request).pipe(catchError(this.handleError));
  }

  private handleError(error: HttpErrorResponse) {
    const message = error.error?.message ?? 'Something went wrong. Please try again.';
    return throwError(() => new Error(message));
  }
}
