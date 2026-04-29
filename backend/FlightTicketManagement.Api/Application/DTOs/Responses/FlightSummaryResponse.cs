namespace FlightTicketManagement.Api.Application.DTOs.Responses;

public sealed record FlightSummaryResponse(int FlightNumber, string Destination, int TotalPassengers, int AvailableSeats, Dictionary<string, int> AvailableSeatsByClass);
