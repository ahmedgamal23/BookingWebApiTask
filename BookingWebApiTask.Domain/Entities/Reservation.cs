using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApiTask.Domain.Entities
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ReservedById { get; set; } = string.Empty;  // FK
        [ForeignKey(nameof(ReservedById))]
        public ApplicationUser? ReservedUser { get; set; }

        [Required, MaxLength(200)]
        public string CustomerName { get; set; } = string.Empty;

        public int TripId { get; set; }  // FK
        [ForeignKey(nameof(TripId))]
        public Trip? Trip { get; set; }

        public string? notes { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
