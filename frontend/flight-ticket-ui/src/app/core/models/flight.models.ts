export type FlightClass = 'First' | 'Business' | 'Economy';

export interface AvailableSeatsByClass {
  first: number;
  business: number;
  economy: number;
}

export interface FlightSummary {
  flightNumber: number;
  destination: string;
  totalPassengers: number;
  availableSeats: number;
  availableSeatsByClass: AvailableSeatsByClass;
}

export interface AddFlightRequest {
  flightNumber: number;
  destination: string;
}

export interface AddPassengerRequest {
  firstName: string;
  lastName: string;
  passengerId: string;
  class: FlightClass;
  ticketPrice: number;
  numberOfBags: number;
  totalBaggageWeight: number;
}

export interface AddPassengerResponse {
  seatNumber: number;
  firstName: string;
  lastName: string;
  class: FlightClass;
  message: string;
}

export interface Passenger {
  seatNumber: number;
  firstName: string;
  lastName: string;
  passengerId: string;
  class: FlightClass;
  ticketPrice: number;
  numberOfBags: number;
  totalBaggageWeight: number;
}
