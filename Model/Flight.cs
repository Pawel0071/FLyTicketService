using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FLyTicketService.Model
{
    public class Flight : IValidatableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid FlyId { get; set; }

        [Required]
        public required Operator Operator { get; set; }

        [Required]
        public required Aircraft Aircraft { get; set; }

        [Required]
        [StringLength(10, ErrorMessage = "Fly number cannot be longer than 10 characters.")]
        public required string FlyNumber { get; set; }

        [Required]
        public DateTime Departure { get; set; }

        [Required]
        public DateTime Arrival { get; set; }

        [Required]
        public required AirPort Origin { get; set; }

        [Required]
        public required AirPort Destination { get; set; }

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