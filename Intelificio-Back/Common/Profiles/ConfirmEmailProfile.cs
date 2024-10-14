using AutoMapper;
using Backend.Features.Authentication.Commands.ConfirmEmail;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class ConfirmEmailProfile : Profile
    {
        public ConfirmEmailProfile()
        {
            // Mapea el objeto User al ConfirmEmailUserCommandResponse
            CreateMap<User, ConfirmEmailUserCommandResponse>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)); // Mapea Email directamente
        }
    }
}
