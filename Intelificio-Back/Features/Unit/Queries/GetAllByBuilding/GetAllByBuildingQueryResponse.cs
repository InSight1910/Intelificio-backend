namespace Backend.Features.Unit.Queries.GetAllByBuilding
{
    public class GetAllByBuildingQueryResponse
    {
        public required int Id { get; set; }
        public required string UnitType { get; set; }
        public required string Number { get; set; }
        public required string Building { get; set; }
        public required int Floor { get; set; }
        public required float Surface { get; set; }
        public required string User { get; set; }
    }
}
