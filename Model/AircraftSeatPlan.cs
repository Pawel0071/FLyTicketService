using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLyTicketService.Model
{
    public class AircraftSeatPlan
    {
        [Key]
        public Guid AircraftSeatPlanId { get; set; }

        [ForeignKey("Aircraft")]
        public Guid AircraftId { get; set; }

        public ICollection<AircraftSeat> Seats { get; set; }

        [NotMapped]
        public int TotalSeats => this.Seats.Count();

        [NotMapped]
        public int AvailableSeats => this.Seats.Count(s => s.IsAvailable);

        [NotMapped]
        public int OccupiedSeats => this.Seats.Count(s => !s.IsAvailable);
    }
}
