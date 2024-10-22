using Backend.Common.Response;
using MediatR;

namespace Backend.Features.AssignedFines.Commands.Update
{
    public class UpdateAssignedFinesCommand : IRequest<Result>
    {
        public int AssignedFineId { get; set; }
        public int FineId { get; set; }
        public int UnitId { get; set; }
        public DateTime EventDate { get; set; }
        public required string Comment { get; set; }
    }
}
