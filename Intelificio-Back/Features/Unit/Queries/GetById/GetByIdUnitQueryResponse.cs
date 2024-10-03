using Backend.Models;

namespace Backend.Features.Unit.Queries.GetById
{
    public class GetByIdUnitQueryResponse
    {
        public required string Number { get; set; }
        public required string UnitType { get; set; }
        public required int UnitTypeId { get; set; }
        public required string Building { get; set; }
        public required int BuildingId { get; set; }
        public required int Floor { get; set; }
        public required float Surface { get; set; }
    }
}
