# Flight Ticket Management System

A complete take-home assessment application for managing flights and passengers with automatic seat assignment and baggage validation.

## Architecture Summary

The solution is split into a .NET Web API backend and an Angular frontend. The backend follows a simple clean-architecture style with Controllers for HTTP concerns, Application services for business rules, Domain entities/rules for core concepts, and an Infrastructure repository for thread-safe in-memory storage. The frontend uses Angular standalone components, Router, Reactive Forms, Angular Material, centralized API access, and snackbar notifications.

## Technology Stack

- Backend: .NET 10 LTS, ASP.NET Core Web API, C#
- Frontend: Angular 21, TypeScript, standalone components
- UI: Angular Material
- Data Store: In-memory only using `ConcurrentDictionary<int, Flight>`
- API Docs: Swagger/OpenAPI with XML comments

## Backend Setup

```bash
cd backend/FlightTicketManagement.Api
dotnet restore
dotnet run
```

Expected URLs:

```text
Backend API: https://localhost:5001 or the port shown by dotnet run
Swagger: https://localhost:5001/swagger or the port shown by dotnet run
```

## Frontend Setup

Update `src/environments/environment.ts` if your backend uses a different HTTPS port.

```bash
cd frontend/flight-ticket-ui
npm install
npm start
```

Expected URL:

```text
Frontend: http://localhost:4200
```

## How to Run the App

1. Start the backend API.
2. Open Swagger and confirm `/api/flights` endpoints are available.
3. Start the Angular frontend.
4. Add a flight from the Flights page.
5. Click the flight row to add and view passengers.

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/flights` | Adds a new flight |
| GET | `/api/flights` | Gets all flights |
| GET | `/api/flights/{flightNumber}` | Gets one flight summary |
| POST | `/api/flights/{flightNumber}/passengers` | Adds a passenger and assigns a seat |
| GET | `/api/flights/{flightNumber}/passengers` | Gets passengers on a flight |

## Business Rules

| Class | Seat Range | Max Bags | Max Total Weight |
|---|---:|---:|---:|
| First | 1-20 | 2 | 30 kg |
| Business | 21-50 | 2 | 20 kg |
| Economy | 51-200 | 1 | 20 kg |

Seat assignment always selects the lowest unused seat number within the selected class range.

## Validation Rules

- Flight number is required and must be positive.
- Destination is required.
- Duplicate flight numbers are blocked.
- Flight must exist before passengers can be added or listed.
- Duplicate passenger IDs are blocked per flight.
- Passenger name, ID, class, ticket price, bags, and baggage weight are validated.
- Ticket price, bags, and baggage weight must be greater than or equal to 0.
- Baggage must comply with selected class rules.
- Full classes return `SeatUnavailable`.

## Design Decisions

- In-memory storage keeps the assessment simple and avoids database setup.
- `ConcurrentDictionary` is used for thread-safe flight storage.
- Business exceptions are normalized into consistent `{ error, message }` JSON responses.
- XML comments are enabled so Swagger shows endpoint documentation.
- Angular Material provides fast, professional, responsive UI components.
- Backend validation remains the source of truth; frontend validation improves UX only.

## Assumptions

- Passenger uniqueness is scoped to a single flight, not globally.
- The backend HTTPS port may differ depending on local launch settings.
- No authentication is required for this assessment.
- Data resets when the backend process restarts.

## Future Improvements

- Add persistent database storage using EF Core.
- Add authentication and role-based authorization.
- Add unit and integration tests.
- Add pagination and server-side sorting.
- Add audit logs for flight and passenger changes.
- Add CI/CD pipeline and Docker support.
