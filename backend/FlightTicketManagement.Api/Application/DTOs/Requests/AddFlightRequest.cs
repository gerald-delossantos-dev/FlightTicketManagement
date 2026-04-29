using System.ComponentModel.DataAnnotations;

namespace FlightTicketManagement.Api.Application.DTOs.Requests;

public sealed class AddFlightRequest
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Flight number must be positive.")]
    public int FlightNumber { get; init; }

    [Required]
    [MinLength(1)]
    public string Destination { get; init; } = string.Empty;
}
