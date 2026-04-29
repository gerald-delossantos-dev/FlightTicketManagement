using System.ComponentModel.DataAnnotations;
using FlightTicketManagement.Api.Domain.Enums;

namespace FlightTicketManagement.Api.Application.DTOs.Requests;

public sealed class AddPassengerRequest
{
    [Required]
    [MinLength(1)]
    public string FirstName { get; init; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string LastName { get; init; } = string.Empty;

    [Required]
    [MinLength(1)]
    public string PassengerId { get; init; } = string.Empty;

    [Required]
    public FlightClass Class { get; init; }

    [Range(0, double.MaxValue, ErrorMessage = "Ticket price must be greater than or equal to 0.")]
    public decimal TicketPrice { get; init; }

    [Range(0, int.MaxValue, ErrorMessage = "Number of bags must be greater than or equal to 0.")]
    public int NumberOfBags { get; init; }

    [Range(0, double.MaxValue, ErrorMessage = "Total baggage weight must be greater than or equal to 0.")]
    public decimal TotalBaggageWeight { get; init; }
}
