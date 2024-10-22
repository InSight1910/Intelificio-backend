using Backend.Common.Response;
using MediatR;

namespace Backend.Features.AssignedFines.Queries.GetAssignedFinesById
{
    public class GetAssignedFinesByIdQuery : IRequest<Result>
    {
        public int AssignedFineId { get; set; }
    }
}
