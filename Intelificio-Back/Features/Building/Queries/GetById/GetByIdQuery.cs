using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Building.Queries.GetById
{
    public class GetByIdQuery : IRequest<Result>
    {
        public required int BuildingId { get; set; }
    }
}
