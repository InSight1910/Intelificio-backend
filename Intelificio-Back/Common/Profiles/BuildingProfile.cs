using AutoMapper;
using Backend.Features.Buildings.Commands.Create;
using Backend.Features.Buildings.Commands.Update;
using Backend.Features.Buildings.Queries.GetById;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class BuildingProfile : Profile
    {
        public BuildingProfile()
        {

            _ = CreateMap<CreateBuildingCommand, Building>();
            CreateMap<UpdateBuildingCommand, Building>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            _ = CreateMap<Building, GetByIDQueryResponse>();

        }
    }
}
