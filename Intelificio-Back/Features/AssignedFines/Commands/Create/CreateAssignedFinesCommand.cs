using Backend.Common.Response;
using MediatR;

namespace Backend.Features.AssignedFines.Commands.Create
{
    public class CreateAssignedFinesCommand : IRequest<Result>
    {
        public required int FineId { get; set; }
        public required int UnitId { get; set; }
        public DateTime EventDate { get; set; }
        public required string Comment { get; set; }

    }
}
