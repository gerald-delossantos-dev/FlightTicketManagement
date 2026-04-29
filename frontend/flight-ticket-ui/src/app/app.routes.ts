import { Routes } from '@angular/router';
import { FlightsPageComponent } from './features/flights/flights-page.component';
import { PassengersPageComponent } from './features/passengers/passengers-page.component';

export const routes: Routes = [
  { path: '', redirectTo: 'flights', pathMatch: 'full' },
  { path: 'flights', component: FlightsPageComponent },
  { path: 'flights/:flightNumber/passengers', component: PassengersPageComponent },
  { path: '**', redirectTo: 'flights' }
];
