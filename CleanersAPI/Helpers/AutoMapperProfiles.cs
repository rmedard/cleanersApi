using AutoMapper;
using CleanersAPI.Models;
using CleanersAPI.Models.Dtos.Email;
using CleanersAPI.Models.Dtos.Service;
using Microsoft.Extensions.Configuration;

namespace CleanersAPI.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles(IConfiguration configuration)
        {
            var config = configuration;
            CreateMap<Expertise, ExpertiseForServiceCreate>()
                .ForMember(dest => dest.ProfessionalId, opt => { opt.MapFrom(src => src.ProfessionalId); })
                .ForMember(dest => dest.ServiceId, opt => { opt.MapFrom(src => src.ServiceId); })
                .ForAllOtherMembers(opt => opt.Ignore());
            CreateMap<ExpertiseForServiceCreate, Expertise>();
            CreateMap<Reservation, ReservationForCreate>()
                .ForMember(dest => dest.ExpertiseForServiceCreate, opt => { opt.MapFrom(src => src.Expertise); });
            CreateMap<ReservationForCreate, Reservation>()
                .ForMember(dest => dest.Expertise, opt => { opt.MapFrom(src => src.ExpertiseForServiceCreate); })
                .ForMember(dest => dest.EndTime, opt => { opt.MapFrom(src => src.StartTime.AddHours(src.Duration)); })
                .ForMember(dest => dest.Recurrence, opt => {opt.MapFrom(src => src.Recurrence);});
            CreateMap<EmailForSend, Email>()
                .ForMember(dest => dest.From,
                    opt => { opt.MapFrom(src => config.GetValue<string>("SendGrid:senderEmail")); })
                .ForMember(dest => dest.SenderNames,
                    opt => { opt.MapFrom(src => config.GetValue<string>("SendGrid:senderName")); });
        }
    }
}