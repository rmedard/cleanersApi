using System.Linq;
using AutoMapper;
using CleanersAPI.Models;
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
                    opt => { opt.MapFrom(src => src.Roles.Select(x => x.Role).Select(y => y.RoleName)); })
                .ForMember(dest => dest.CustomerId, opt => { opt.MapFrom(src => src.Customer.Id); })
                .ForMember(dest => dest.ProfessionalId, opt => { opt.MapFrom(src => src.Professional.Id); });
            CreateMap<Expertise, ExpertiseForServiceCreate>()
                .ForMember(dest => dest.ProfessionalId, opt => { opt.MapFrom(src => src.ProfessionalId); })
                .ForMember(dest => dest.ServiceId, opt => { opt.MapFrom(src => src.ServiceId); })
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<ExpertiseForServiceCreate, Expertise>();
            CreateMap<Reservation, ReservationForCreate>()
                .ForMember(dest => dest.ExpertiseForServiceCreate, opt => { opt.MapFrom(src => src.Expertise); });
            CreateMap<ReservationForCreate, Reservation>()
                .ForMember(dest => dest.Expertise, opt => { opt.MapFrom(src => src.ExpertiseForServiceCreate); })
                .ForMember(dest => dest.EndTime, opt => { opt.MapFrom(src => src.StartTime.AddHours(src.Duration)); });
        }
    }
}