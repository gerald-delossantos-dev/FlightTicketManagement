import { Component, computed, inject } from '@angular/core';
import { NavigationEnd, Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { filter, map, startWith } from 'rxjs';
import { MatTabsModule } from '@angular/material/tabs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, MatTabsModule],
  template: `
    <div class="app-shell">
      <header class="app-header">
        <div>
          <p class="eyebrow">Airline Operations Tool</p>
          <h1>Flight Ticket Management</h1>
          <p class="subtitle">Manage flights, passengers, automatic seat assignment, and baggage validation.</p>
        </div>
      </header>

      <main class="main-panel">
        <nav mat-tab-nav-bar [tabPanel]="tabPanel" class="feature-tabs">
          <a
            mat-tab-link
            routerLink="/flights"
            routerLinkActive="active-tab"
            [active]="activeTab() === 'flights'">
            Flights
          </a>

          <a
            mat-tab-link
            [routerLink]="passengersLink()"
            routerLinkActive="active-tab"
            [active]="activeTab() === 'passengers'"
            [class.disabled-tab]="!selectedFlightNumber()"
            [attr.aria-disabled]="!selectedFlightNumber()">
            Passengers
          </a>
        </nav>

        <mat-tab-nav-panel #tabPanel>
          <router-outlet />
        </mat-tab-nav-panel>
      </main>
    </div>
  `
})
export class AppComponent {
  private readonly router = inject(Router);

  private readonly currentUrl = toSignal(
    this.router.events.pipe(
      filter((event): event is NavigationEnd => event instanceof NavigationEnd),
      map(event => event.urlAfterRedirects),
      startWith(this.router.url)
    ),
    { initialValue: this.router.url }
  );

  readonly activeTab = computed(() =>
    this.currentUrl().includes('/passengers') ? 'passengers' : 'flights'
  );

  readonly selectedFlightNumber = computed(() => {
    const match = this.currentUrl().match(/\/flights\/(\d+)\/passengers/);
    return match ? Number(match[1]) : null;
  });

  readonly passengersLink = computed(() => {
    const flightNumber = this.selectedFlightNumber();
    return flightNumber ? ['/flights', flightNumber, 'passengers'] : ['/flights'];
  });
}
