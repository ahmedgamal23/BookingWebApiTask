using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BookingWebApiTask.Application.Dtos
{
    public class TripDto
    {
        [Required, MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string CityName { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public string? ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
