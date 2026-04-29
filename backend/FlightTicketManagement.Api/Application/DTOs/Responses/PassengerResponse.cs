using FlightTicketManagement.Api.Domain.Enums;

namespace FlightTicketManagement.Api.Application.DTOs.Responses;

public sealed record PassengerResponse(int SeatNumber, string FirstName, string LastName, string PassengerId, FlightClass Class, decimal TicketPrice, int NumberOfBags, decimal TotalBaggageWeight);
