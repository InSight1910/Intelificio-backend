using Backend.Common.Response;
using MediatR;

namespace Backend.Features.Unit.Commands.Create
{
    public class CreateUnitCommand : IRequest<Result>
    {
        public required string Number { get; set; }
        public required int Floor { get; set; }
        public required float Surface { get; set; }
        public required int UnitTypeId { get; set; }
        public required int BuildingId { get; set; }
        public required int UserId { get; set; }

    }
}
