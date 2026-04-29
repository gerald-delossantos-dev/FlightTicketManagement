using FlightTicketManagement.Api.Domain.Enums;

namespace FlightTicketManagement.Api.Domain.Rules;

public sealed record FlightClassRule(FlightClass Class, int SeatStart, int SeatEnd, int MaxBags, decimal MaxTotalWeight)
{
    public int Capacity => SeatEnd - SeatStart + 1;

    public static FlightClassRule For(FlightClass flightClass) => flightClass switch
    {
        FlightClass.First => new FlightClassRule(FlightClass.First, 1, 20, 2, 30m),
        FlightClass.Business => new FlightClassRule(FlightClass.Business, 21, 50, 2, 20m),
        FlightClass.Economy => new FlightClassRule(FlightClass.Economy, 51, 200, 1, 20m),
        _ => throw new ArgumentOutOfRangeException(nameof(flightClass), flightClass, null)
    };

    public static IReadOnlyList<FlightClassRule> All { get; } =
    [
        For(FlightClass.First),
        For(FlightClass.Business),
        For(FlightClass.Economy)
    ];
}
