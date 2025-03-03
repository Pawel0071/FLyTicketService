using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLyTicketService.Model
{
    public class Flight : IValidatableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FlightId { get; set; } = Guid.NewGuid();

        [Required]
        public required Airline Airline { get; set; }

        [Required]
        public required Aircraft Aircraft { get; set; }


        public required ICollection<FlightSeat> Seats { get; set; }

        [NotMapped]
        public int TotalSeats => this.Seats.Count;

        [NotMapped]
        public int AvailableSeats => this.Seats.Count(s => s.IsAvailable);

        [NotMapped]
        public int OccupiedSeats => this.Seats.Count(s => !s.IsAvailable);

        [Required]
        [StringLength(10, ErrorMessage = "Fly number cannot be longer than 10 characters.")]
        public required string FlyNumber { get; set; }

        [Required]
        public DateTime Departure { get; set; }

        [Required]
        public DateTime Arrival { get; set; }

        [Required]
        public required Airport Origin { get; set; }

        [Required]
        public required Airport Destination { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Departure >= Arrival)
            {
                yield return new ValidationResult("Departure must be before Arrival.", new[] { nameof(Departure), nameof(Arrival) });
            }

            if (Origin == Destination)
            {
                yield return new ValidationResult("Origin and Destination cannot be the same.", new[] { nameof(Origin), nameof(Destination) });
            }
        }
    }
}