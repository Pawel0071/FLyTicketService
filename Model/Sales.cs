namespace FLyTicketService.Model
{
    public class Sales
    {
        public int SaleId { get; set; }
        public int TicketId { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal Amount { get; set; }
    }
    public class AircraftSeat
    {
        public int SeatId { get; set; }
        public string SeatNumber { get; set; }
        public string Class { get; set; }
        public bool IsAvailable { get; set; }
    }
    public class FlyTicket
    {
        public int TicketId { get; set; }
        public int FlightId { get; set; }
        public int PassengerId { get; set; }
        public int SeatId { get; set; }
        public DateTime IssueDate { get; set; }
    }
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int PassengerId { get; set; }
        public int FlightId { get; set; }
        public DateTime ReservationDate { get; set; }
        public bool IsConfirmed { get; set; }
    }
    public class SaleTicket
    {
        public int SaleTicketId { get; set; }
        public int TicketId { get; set; }
        public int SaleId { get; set; }
    }
}
