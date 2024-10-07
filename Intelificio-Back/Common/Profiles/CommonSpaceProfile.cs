using AutoMapper;
using Backend.Features.Buildings.Commands.Create;
using Backend.Features.Buildings.Commands.Update;
using Backend.Features.Buildings.Queries.GetById;
using Backend.Features.CommonSpaces.Commands.Create;
using Backend.Features.CommonSpaces.Commands.Update;
using Backend.Models;

namespace Backend.Common.Profiles;

public class CommonSpaceProfile : Profile
{
    public CommonSpaceProfile()
    {
        CreateMap<CreateCommonSpaceCommand, CommonSpace>();
        CreateMap<CommonSpace, CreateCommonSpaceCommandResponse>();
        CreateMap<UpdateCommonSpaceCommand, CommonSpace>();
    }
}