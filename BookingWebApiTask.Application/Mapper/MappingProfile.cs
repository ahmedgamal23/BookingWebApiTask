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
            CreateMap<Reservation, ReservationDto>().ReverseMap()
                .ForAllMembers(opt => opt.Condition(
                    (src, dst, value) => value != null && (value is not int)
                ));
            CreateMap<Trip, TripDto>()
                .ReverseMap()
                .ForAllMembers(
                    opt => opt.Condition(
                        // when price become null, it will be set to 0
                        (src, dts , value) => value != null && (value is not double)
                    )
                );
            CreateMap<ApplicationUser, RegisterDto>().ReverseMap();
            CreateMap<ReservationResult, Reservation>().ReverseMap()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ReservedUser!.Name));
                
            CreateMap<ApplicationUser, IdentityUserResult>().ReverseMap();
        }

    }
}
