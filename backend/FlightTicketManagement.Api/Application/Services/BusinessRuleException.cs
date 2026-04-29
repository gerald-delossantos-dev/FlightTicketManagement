namespace FlightTicketManagement.Api.Application.Services;

public sealed class BusinessRuleException(string error, string message) : Exception(message)
{
    public string Error { get; } = error;
}
