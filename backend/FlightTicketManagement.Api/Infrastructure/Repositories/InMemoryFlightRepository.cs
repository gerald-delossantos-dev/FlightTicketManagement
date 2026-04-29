using System.Collections.Concurrent;
using FlightTicketManagement.Api.Application.Interfaces;
using FlightTicketManagement.Api.Domain.Entities;

namespace FlightTicketManagement.Api.Infrastructure.Repositories;

public sealed class InMemoryFlightRepository : IFlightRepository
{
    private readonly ConcurrentDictionary<int, Flight> _flights = new();

    public bool TryAdd(Flight flight) => _flights.TryAdd(flight.FlightNumber, flight);
    public Flight? GetByFlightNumber(int flightNumber) => _flights.TryGetValue(flightNumber, out var flight) ? flight : null;
    public IReadOnlyList<Flight> GetAll() => _flights.Values.ToList();
}
