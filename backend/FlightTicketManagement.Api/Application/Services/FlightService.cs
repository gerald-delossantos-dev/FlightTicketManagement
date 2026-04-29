using FlightTicketManagement.Api.Application.DTOs.Requests;
using FlightTicketManagement.Api.Application.DTOs.Responses;
using FlightTicketManagement.Api.Application.Interfaces;
using FlightTicketManagement.Api.Domain.Entities;
using FlightTicketManagement.Api.Domain.Enums;
using FlightTicketManagement.Api.Domain.Rules;

namespace FlightTicketManagement.Api.Application.Services;

public sealed class FlightService(IFlightRepository repository, ISeatAssignmentService seatAssignmentService) : IFlightService
{
    public FlightSummaryResponse AddFlight(AddFlightRequest request)
    {
        var destination = request.Destination.Trim();
        var flight = new Flight(request.FlightNumber, destination);
        if (!repository.TryAdd(flight))
        {
            throw new BusinessRuleException("DuplicateFlight", $"Flight number {request.FlightNumber} already exists.");
        }
        return ToSummary(flight);
    }

    public IReadOnlyList<FlightListResponse> GetFlights() => repository.GetAll()
        .OrderBy(f => f.FlightNumber)
        .Select(f =>
        {
            var availableByClass = seatAssignmentService.GetAvailableSeatsByClass(f);
            return new FlightListResponse(f.FlightNumber, f.Destination, f.GetPassengersSnapshot().Count, availableByClass.Values.Sum(), availableByClass);
        })
        .ToList();

    public FlightSummaryResponse GetFlight(int flightNumber) => ToSummary(GetFlightOrThrow(flightNumber));

    public AddPassengerResponse AddPassenger(int flightNumber, AddPassengerRequest request)
    {
        var flight = GetFlightOrThrow(flightNumber);
        var firstName = request.FirstName.Trim();
        var lastName = request.LastName.Trim();
        var passengerId = request.PassengerId.Trim();

        if (flight.HasPassenger(passengerId))
        {
            throw new BusinessRuleException("DuplicatePassenger", $"Passenger with ID {passengerId} already exists on flight {flightNumber}.");
        }

        ValidateBaggage(request);
        var seatNumber = seatAssignmentService.AssignNextAvailableSeat(flight, request.Class);
        if (seatNumber is null)
        {
            throw new BusinessRuleException("SeatUnavailable", $"No available seats in {FormatClass(request.Class)} class.");
        }

        var passenger = new Passenger
        {
            FirstName = firstName,
            LastName = lastName,
            PassengerId = passengerId,
            Class = request.Class,
            TicketPrice = request.TicketPrice,
            NumberOfBags = request.NumberOfBags,
            TotalBaggageWeight = request.TotalBaggageWeight,
            SeatNumber = seatNumber.Value
        };

        flight.AddPassenger(passenger);
        return new AddPassengerResponse(passenger.SeatNumber, passenger.FirstName, passenger.LastName, passenger.Class, "Passenger added successfully");
    }

    public IReadOnlyList<PassengerResponse> GetPassengers(int flightNumber)
    {
        var flight = GetFlightOrThrow(flightNumber);
        return flight.GetPassengersSnapshot()
            .Select(p => new PassengerResponse(p.SeatNumber, p.FirstName, p.LastName, p.PassengerId, p.Class, p.TicketPrice, p.NumberOfBags, p.TotalBaggageWeight))
            .ToList();
    }

    private Flight GetFlightOrThrow(int flightNumber) => repository.GetByFlightNumber(flightNumber)
        ?? throw new BusinessRuleException("FlightNotFound", $"Flight number {flightNumber} does not exist.");

    private FlightSummaryResponse ToSummary(Flight flight)
    {
        var availableByClass = seatAssignmentService.GetAvailableSeatsByClass(flight);
        return new FlightSummaryResponse(flight.FlightNumber, flight.Destination, flight.GetPassengersSnapshot().Count, availableByClass.Values.Sum(), availableByClass);
    }

    private static void ValidateBaggage(AddPassengerRequest request)
    {
        var rule = FlightClassRule.For(request.Class);
        if (request.NumberOfBags > rule.MaxBags || request.TotalBaggageWeight > rule.MaxTotalWeight)
        {
            throw new BusinessRuleException(
                "BaggageExceeded",
                $"{FormatClass(request.Class)} class allows maximum {rule.MaxBags} bags with total weight of {rule.MaxTotalWeight:g} kg. Provided: {request.NumberOfBags} bags, {request.TotalBaggageWeight:g} kg.");
        }
    }

    private static string FormatClass(FlightClass flightClass) => flightClass switch
    {
        FlightClass.First => "First",
        FlightClass.Business => "Business",
        FlightClass.Economy => "Economy",
        _ => flightClass.ToString()
    };
}
