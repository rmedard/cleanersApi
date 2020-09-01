using System;
using System.Collections.ObjectModel;
using System.Linq;
using AutoMapper;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos;
using CleanersAPI.Models.Dtos.Service;
using CleanersAPI.Models.Dtos.User;

namespace CleanersAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForDisplayDto>()
                .ForMember(dest => dest.Roles,
                    opt => { opt.MapFrom(src => src.Roles.Select(x => x.role).Select(y => y.RoleName)); })
                .ForMember(dest => dest.CustomerId, opt => { opt.MapFrom(src => src.Customer.Id); })
                .ForMember(dest => dest.ProfessionalId, opt => { opt.MapFrom(src => src.Professional.Id); });
            CreateMap<Expertise, ExpertiseForServiceCreate>()
                .ForMember(dest => dest.ProfessionalId, opt => {opt.MapFrom(src => src.ProfessionalId);})
                .ForMember(dest => dest.ProfessionId, opt => {opt.MapFrom(src => src.ServiceId);})
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<ExpertiseForServiceCreate, Expertise>();
            CreateMap<Reservation, ServiceForCreate>()
                .ForMember(dest => dest.ExpertiseForServiceCreate, opt => { opt.MapFrom(src => src.Expertise); });
            CreateMap<ServiceForCreate, Reservation>()
                .ForMember(dest => dest.Expertise, opt => {opt.MapFrom(src => src.ExpertiseForServiceCreate);});
        }
    }
}