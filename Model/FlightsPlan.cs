using System.ComponentModel.DataAnnotations;

namespace FLyTicketService.Model
{
    public class FlightsPlan: IValidatableObject
    {
        public Guid FlightsPlanId { get; set; } = Guid.NewGuid();
        public required Airline Airline { get; set; }
        public required Aircraft Aircraft { get; set; }
        public required ICollection<FlightSeat> Seats { get; set; }
        public int TotalSeats => this.Seats.Count;
        public int AvailableSeats => this.Seats.Count(s => s.IsAvailable);
        public int OccupiedSeats => this.Seats.Count(s => !s.IsAvailable);
        public required string FlyNumber { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public required Airport Origin { get; set; }
        public required Airport Destination { get; set; }
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