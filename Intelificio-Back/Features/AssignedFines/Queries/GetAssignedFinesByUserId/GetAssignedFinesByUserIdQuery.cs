using Backend.Common.Response;
using MediatR;

namespace Backend.Features.AssignedFines.Queries.GetAssignedFinesByUserId
{
    public class GetAssignedFinesByUserIdQuery : IRequest<Result>
    {
        public int UserId { get; set; }
    }
}
