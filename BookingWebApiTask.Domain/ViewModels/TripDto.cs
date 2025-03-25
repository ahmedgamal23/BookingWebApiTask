using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace BookingWebApiTask.Application.Dtos
{
    public class TripDto
    {
        public string? Name { get; set; }

        public string? CityName { get; set; }

        public double? Price { get; set; }

        public string? Content { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;

        public string? ImageUrl { get; set; }

        public IFormFile? ImageFile { get; set; }
    }
}
