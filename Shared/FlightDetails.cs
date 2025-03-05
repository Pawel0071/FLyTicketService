using FLyTicketService.Model;

namespace FLyTicketService.Shared
{
    public record FlightDetails(Airline? Airline,
        Aircraft? Aircraft,
        Airport? Origin,
        Airport? Destination);
}

