using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLyTicketService.Model
{
    public class Aircraft
    {
        [Key]
        public Guid AircraftId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Model { get; set; }

        [Required]
        [MaxLength(10)]
        public string RegistrationNumber { get; set; }

        public int Capacity => this.SeatPlan.AvailableSeats;

        [ForeignKey("AircraftSeatPlanId")]
        public AircraftSeatPlan SeatPlan { get; set; }
    }

}
