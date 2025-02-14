using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLyTicketService.Model
{
    public class FlightSeatPlan
    {
        public FlightSeatPlan(ICollection<FlightSeat> seats) {
            this.Seats = seats;
        }

        [Key]
        public Guid FlightSeatPlanId { get; set; }

        [ForeignKey("Flight")]
        public Guid FlightId { get; set; }

        public ICollection<FlightSeat> Seats { get; set; }

        [NotMapped]
        public int TotalSeats => Seats.Count;

        [NotMapped]
        public int AvailableSeats => Seats.Count(s => s.IsAvailable);

        [NotMapped]
        public int OccupiedSeats => Seats.Count(s => !s.IsAvailable);
    }
}
