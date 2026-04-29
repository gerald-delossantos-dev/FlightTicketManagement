using FlightTicketManagement.Api.Domain.Enums;

namespace FlightTicketManagement.Api.Application.DTOs.Responses;

public sealed record AddPassengerResponse(int SeatNumber, string FirstName, string LastName, FlightClass Class, string Message);
