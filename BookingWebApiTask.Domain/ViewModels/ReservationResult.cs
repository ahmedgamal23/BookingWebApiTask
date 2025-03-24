using BookingWebApiTask.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApiTask.Domain.ViewModels
{
    public class ReservationResult
    {
        public string Name { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public Trip? Trip { get; set; }
        public string? notes { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}
