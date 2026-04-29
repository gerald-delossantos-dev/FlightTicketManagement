using FlightTicketManagement.Api.Application.DTOs.Requests;
using FlightTicketManagement.Api.Application.DTOs.Responses;

namespace FlightTicketManagement.Api.Application.Interfaces;

public interface IFlightService
{
    FlightSummaryResponse AddFlight(AddFlightRequest request);
    IReadOnlyList<FlightListResponse> GetFlights();
    FlightSummaryResponse GetFlight(int flightNumber);
    AddPassengerResponse AddPassenger(int flightNumber, AddPassengerRequest request);
    IReadOnlyList<PassengerResponse> GetPassengers(int flightNumber);
}
