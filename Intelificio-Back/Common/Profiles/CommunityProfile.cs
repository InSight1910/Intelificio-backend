using AutoMapper;
using Backend.Features.Community.Commands.Create;
using Backend.Features.Community.Commands.Update;
using Backend.Features.Community.Queries.GetAllByUser;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class CommunityProfile : Profile
    {
        public CommunityProfile()
        {
            _ = CreateMap<Community, GetAllByUserResponse>()
                .ForMember(ent => ent.Name, res => res.MapFrom(p => p.Name))
                .ForMember(ent => ent.BuildingCount, res => res.MapFrom(p => p.Buildings.Count()))
                .ForMember(ent => ent.UnitCount, res => res.MapFrom(p => p.Buildings.Sum(item => item.Units.Count())));
            _ = CreateMap<CreateCommunityCommand, Community>();
            CreateMap<UpdateCommunityCommand, Community>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
