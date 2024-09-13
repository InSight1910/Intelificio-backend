using AutoMapper;
using Backend.Features.Unit.Commands.Update;
using Backend.Features.Unit.Queries.GetByID;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class UnitProfile : Profile
    {
        protected UnitProfile()
        {
            CreateMap<UpdateUnitCommand, Unit>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
