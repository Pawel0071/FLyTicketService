using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLyTicketService.Model
{
    public class Aircraft
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid AircraftId { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(100)]
        public required string Model { get; set; }

        [Required]
        [MaxLength(10)]
        public required string RegistrationNumber { get; set; }

        public ICollection<AircraftSeat> Seats { get; set; } = new List<AircraftSeat>();

        [NotMapped]
        public int TotalSeats => this.Seats.Count( x => !x.OutOfService);
    }

}
