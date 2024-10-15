using AutoMapper;
using Backend.Features.Guest.Commands.Create;
using Backend.Features.Guest.Commands.Update;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class GuestProfile : Profile
    {
        public GuestProfile()
        {
            CreateMap<CreateGuestCommand, Guest>()
                .ForMember(opt => opt.ID, dest => dest.Ignore())
                .ForMember(opt => opt.EntryTime, opt => opt.MapFrom(src => DateTime.UtcNow)); // Asignar la hora actual
            CreateMap<UpdateGuestCommand, Guest>()
                .ForMember(dest => dest.ID, opt => opt.Ignore()) 
                .ForMember(dest => dest.Unit, opt => opt.Ignore())  
                .ForMember(dest => dest.EntryTime, opt => opt.Ignore());  
        }
    }
}
