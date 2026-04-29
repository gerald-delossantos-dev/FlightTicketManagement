using FlightTicketManagement.Api.Domain.Enums;

namespace FlightTicketManagement.Api.Domain.Entities;

public sealed class Passenger
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string PassengerId { get; init; }
    public FlightClass Class { get; init; }
    public decimal TicketPrice { get; init; }
    public int NumberOfBags { get; init; }
    public decimal TotalBaggageWeight { get; init; }
    public int SeatNumber { get; init; }
}
