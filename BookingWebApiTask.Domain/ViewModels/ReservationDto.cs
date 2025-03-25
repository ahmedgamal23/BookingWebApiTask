using System;
using System.ComponentModel.DataAnnotations;

namespace BookingWebApiTask.Application.Dtos
{
    public class ReservationDto
    {
        public string? ReservedById { get; set; }

        public string? CustomerName { get; set; }

        public int? TripId { get; set; }

        public string? Notes { get; set; }

        public DateTime ReservationDate { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
