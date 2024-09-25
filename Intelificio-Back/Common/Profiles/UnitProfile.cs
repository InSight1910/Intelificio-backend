using AutoMapper;
using Backend.Features.Unit.Commands.Create;
using Backend.Features.Unit.Commands.Update;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class UnitProfile : Profile
    {
        public UnitProfile()
        {
            _ = CreateMap<CreateUnitCommand, Unit>()
                .ForMember(opt => opt.Building, dest => dest.Ignore())
                .ForMember(opt => opt.ID, dest => dest.Ignore())
                .ForMember(opt => opt.UnitType, dest => dest.Ignore());
            CreateMap<UpdateUnitCommand, Unit>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}