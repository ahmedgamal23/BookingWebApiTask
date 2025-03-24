using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BookingWebApiTask.Domain.Entities
{
    public class Trip
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string CityName { get; set; } = string.Empty;

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;
        public DateTime Creation_date { get; set; } = DateTime.Now;
    }
}

