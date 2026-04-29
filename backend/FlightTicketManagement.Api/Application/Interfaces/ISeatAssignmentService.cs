using FlightTicketManagement.Api.Domain.Entities;
using FlightTicketManagement.Api.Domain.Enums;

namespace FlightTicketManagement.Api.Application.Interfaces;

public interface ISeatAssignmentService
{
    int? AssignNextAvailableSeat(Flight flight, FlightClass flightClass);
    Dictionary<string, int> GetAvailableSeatsByClass(Flight flight);
}
