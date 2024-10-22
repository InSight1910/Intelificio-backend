using Backend.Common.Response;
using MediatR;

namespace Backend.Features.AssignedFines.Queries.GetAssignedFinesByUnitId
{
    public class GetAssignedFinesByUnitIdQuery : IRequest<Result>
    {
        public int UnitId { get; set; }
    }
}
