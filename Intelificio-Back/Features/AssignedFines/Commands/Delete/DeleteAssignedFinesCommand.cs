using Backend.Common.Response;
using MediatR;

namespace Backend.Features.AssignedFines.Commands.Delete;

public class DeleteAssignedFinesCommand : IRequest<Result>
{
    public int AssignedfineId { get; }
}