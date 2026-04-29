using FlightTicketManagement.Api.Application.DTOs.Requests;
using FlightTicketManagement.Api.Application.DTOs.Responses;
using FlightTicketManagement.Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FlightTicketManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class FlightsController(IFlightService flightService) : ControllerBase
{
    /// <summary>
    /// Adds a new flight to the in-memory flight store.
    /// </summary>
    /// <param name="request">The flight creation request containing flight number and destination.</param>
    /// <returns>The newly created flight summary.</returns>
    /// <response code="201">Flight was successfully created.</response>
    /// <response code="400">Flight number already exists or request is invalid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(FlightSummaryResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<FlightSummaryResponse> AddFlight([FromBody] AddFlightRequest request)
    {
        var response = flightService.AddFlight(request);
        return CreatedAtAction(nameof(GetFlight), new { flightNumber = response.FlightNumber }, response);
    }

    /// <summary>
    /// Gets all flights with passenger counts and seat availability summary.
    /// </summary>
    /// <returns>A list of all flights.</returns>
    /// <response code="200">Flights were retrieved successfully.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FlightListResponse>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<FlightListResponse>> GetFlights()
    {
        return Ok(flightService.GetFlights());
    }

    /// <summary>
    /// Gets a specific flight by flight number.
    /// </summary>
    /// <param name="flightNumber">The flight number to retrieve.</param>
    /// <returns>The matching flight summary.</returns>
    /// <response code="200">Flight was found.</response>
    /// <response code="400">Flight number does not exist.</response>
    [HttpGet("{flightNumber:int}")]
    [ProducesResponseType(typeof(FlightSummaryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    public ActionResult<FlightSummaryResponse> GetFlight([FromRoute] int flightNumber)
    {
        return Ok(flightService.GetFlight(flightNumber));
    }
}
