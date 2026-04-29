using FlightTicketManagement.Api.Application.DTOs.Requests;
using FlightTicketManagement.Api.Application.DTOs.Responses;
using FlightTicketManagement.Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlightTicketManagement.Api.Controllers;

[ApiController]
[Route("api/flights/{flightNumber:int}/passengers")]
public sealed class PassengersController(IFlightService flightService) : ControllerBase
{
    /// <summary>
    /// Adds a passenger to a flight and automatically assigns the next available seat in the selected class.
    /// </summary>
    /// <param name="flightNumber">The flight number where the passenger will be added.</param>
    /// <param name="request">The passenger details, ticket, class, and baggage information.</param>
    /// <returns>The assigned seat number and passenger confirmation message.</returns>
    /// <response code="200">Passenger was successfully added and assigned a seat.</response>
    /// <response code="400">Flight does not exist, passenger is duplicate, baggage exceeded, or no seat is available.</response>
    [HttpPost]
    [ProducesResponseType(typeof(AddPassengerResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<AddPassengerResponse> AddPassenger([FromRoute] int flightNumber, [FromBody] AddPassengerRequest request)
    {
        return Ok(flightService.AddPassenger(flightNumber, request));
    }

    /// <summary>
    /// Gets all passengers assigned to a specific flight.
    /// </summary>
    /// <param name="flightNumber">The flight number whose passengers will be retrieved.</param>
    /// <returns>A list of passengers assigned to the flight.</returns>
    /// <response code="200">Passengers were retrieved successfully.</response>
    /// <response code="400">Flight number does not exist.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PassengerResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<PassengerResponse>> GetPassengers([FromRoute] int flightNumber)
    {
        return Ok(flightService.GetPassengers(flightNumber));
    }
}
