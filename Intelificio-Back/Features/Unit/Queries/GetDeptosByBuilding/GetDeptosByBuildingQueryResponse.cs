namespace Backend.Features.Unit.Queries.GetDeptosByBuilding {
    public class GetDeptosByBuildingQueryResponse
    {
        public required int Id { get; set; }
        public required string Number { get; set; }
        public required string Building { get; set; }
        public required int Floor { get; set; }
        public required float Surface { get; set; }
    }
}
