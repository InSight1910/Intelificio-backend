using AutoMapper;
using Backend.Features.Building.Commands.Create;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class BuildingProfile : Profile
    {
        public BuildingProfile() {

            _ = CreateMap<Building, CreateBuildingCommand>();
        }
    }
}
