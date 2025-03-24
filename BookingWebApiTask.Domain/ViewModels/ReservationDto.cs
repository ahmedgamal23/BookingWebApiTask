using System;
using System.ComponentModel.DataAnnotations;

namespace BookingWebApiTask.Application.Dtos
{
    public class ReservationDto
    {
        [Required]
        public string ReservedById { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string CustomerName { get; set; } = string.Empty;

        [Required]
        public int TripId { get; set; }

        public string? Notes { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
