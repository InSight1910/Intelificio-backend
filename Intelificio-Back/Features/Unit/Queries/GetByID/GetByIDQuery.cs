using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Unit.Queries.GetByID
{
    public class GetByIDQuery : IRequest<Result>
    {
        public required int UnitId { get; set; }
    }
}
