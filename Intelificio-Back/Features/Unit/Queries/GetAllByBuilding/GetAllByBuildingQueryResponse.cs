using Backend.Models;

namespace Backend.Features.Unit.Queries.GetAllByBuilding
{
    public class GetAllByBuildingQueryResponse
    {
        public required UnitType UnitType { get; set; }
        public required string Number { get; set; }
        public required Building Building { get; set; }
        public required int Floor { get; set; }
        public required float Surface { get; set; }
    }
}
