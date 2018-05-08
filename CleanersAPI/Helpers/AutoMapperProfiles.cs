using System;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos;

namespace CleanersAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForDisplayDto>().ForMember(dest => dest.Roles, opt =>
            {
                opt.MapFrom(src => src.Roles.Select(x => x.role).Select(y => y.Name));
            }).ForMember(dest => dest.customerId, opt =>
            {
                opt.MapFrom(src => src.Customer.Id);
            }).ForMember(dest => dest.professionalId, opt =>
            {
                opt.MapFrom(src => src.Professional.Id);
            });
        }
    }
}