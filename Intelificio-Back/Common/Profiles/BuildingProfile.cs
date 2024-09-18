using AutoMapper;
using Backend.Features.Building.Commands.Create;
using Backend.Features.Building.Commands.Update;
using Backend.Features.Building.Queries.GetById;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class BuildingProfile : Profile
    {
        public BuildingProfile() {

            _ = CreateMap<CreateBuildingCommand, Building>();
                CreateMap<UpdateBuildingCommand, Building>()
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));
            _ = CreateMap<Building, GetByIDQueryResponse>();

        }
    }
}
