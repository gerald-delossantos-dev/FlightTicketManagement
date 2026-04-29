namespace FlightTicketManagement.Api.Domain.Entities;

public sealed class Flight
{
    private readonly List<Passenger> _passengers = [];
    private readonly object _sync = new();

    public Flight(int flightNumber, string destination)
    {
        FlightNumber = flightNumber;
        Destination = destination;
    }

    public int FlightNumber { get; }
    public string Destination { get; }

    public IReadOnlyList<Passenger> GetPassengersSnapshot()
    {
        lock (_sync)
        {
            return _passengers.OrderBy(p => p.SeatNumber).ToList();
        }
    }

    public bool HasPassenger(string passengerId)
    {
        lock (_sync)
        {
            return _passengers.Any(p => string.Equals(p.PassengerId, passengerId, StringComparison.OrdinalIgnoreCase));
        }
    }

    public void AddPassenger(Passenger passenger)
    {
        lock (_sync)
        {
            _passengers.Add(passenger);
        }
    }
}
