using FlightTicketManagement.Api.Application.Interfaces;
using FlightTicketManagement.Api.Domain.Entities;
using FlightTicketManagement.Api.Domain.Enums;
using FlightTicketManagement.Api.Domain.Rules;

namespace FlightTicketManagement.Api.Application.Services;

public sealed class SeatAssignmentService : ISeatAssignmentService
{
    public int? AssignNextAvailableSeat(Flight flight, FlightClass flightClass)
    {
        var rule = FlightClassRule.For(flightClass);
        var usedSeats = flight.GetPassengersSnapshot().Select(p => p.SeatNumber).ToHashSet();

        for (var seat = rule.SeatStart; seat <= rule.SeatEnd; seat++)
        {
            if (!usedSeats.Contains(seat))
            {
                return seat;
            }
        }

        return null;
    }

    public Dictionary<string, int> GetAvailableSeatsByClass(Flight flight)
    {
        var passengers = flight.GetPassengersSnapshot();
        return FlightClassRule.All.ToDictionary(
            rule => rule.Class.ToString().ToLowerInvariant(),
            rule => rule.Capacity - passengers.Count(p => p.Class == rule.Class));
    }
}
