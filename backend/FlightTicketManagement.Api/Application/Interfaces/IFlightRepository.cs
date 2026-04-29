using FlightTicketManagement.Api.Domain.Entities;

namespace FlightTicketManagement.Api.Application.Interfaces;

public interface IFlightRepository
{
    bool TryAdd(Flight flight);
    Flight? GetByFlightNumber(int flightNumber);
    IReadOnlyList<Flight> GetAll();
}
