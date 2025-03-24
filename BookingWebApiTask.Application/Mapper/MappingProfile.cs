using AutoMapper;
using BookingWebApiTask.Application.Dtos;
using BookingWebApiTask.Domain.Entities;
using BookingWebApiTask.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApiTask.Application.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Reservation, ReservationDto>().ReverseMap();
            CreateMap<Trip, TripDto>().ReverseMap();
            CreateMap<ApplicationUser, RegisterDto>().ReverseMap();
            CreateMap<ReservationResult, Reservation>().ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ReservedUser!.Name));
        }

    }
}
