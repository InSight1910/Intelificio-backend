using AutoMapper;
using Backend.Features.Fine.Commands.Create;
using Backend.Features.Fine.Commands.Update;
using Backend.Features.Fine.Queries.GetAllByCommunity;
using Backend.Features.Fine.Queries.GetFineById;
using Backend.Models;

namespace Backend.Common.Profiles
{
    public class FineProfile : Profile
    {
        public FineProfile()
        {
            _ = CreateMap<CreateFineCommand, Fine>();
            _ = CreateMap<UpdateFineCommand, Fine>();
            _ = CreateMap<UpdateFineResponse, Fine>();
            _ = CreateMap<Fine, GetFineByIdQueryResponse>();
            _ = CreateMap<Fine, GetAllFinesByCommunityQuery>();
            _ = CreateMap<Fine, GetFineByIdQuery>();
        }
    }
}
