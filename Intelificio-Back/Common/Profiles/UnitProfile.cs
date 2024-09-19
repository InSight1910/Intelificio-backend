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
            _ = CreateMap<CreateUnitCommand, Unit>();
                CreateMap<UpdateUnitCommand, Unit>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}