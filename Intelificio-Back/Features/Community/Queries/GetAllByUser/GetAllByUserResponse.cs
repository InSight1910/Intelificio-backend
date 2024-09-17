namespace Backend.Features.Community.Queries.GetAllByUser
{
    public class GetAllByUserResponse
    {
        public required string Name { get; set; }
        public required string Address { get; set; }
        public required int UnitCount { get; set; }
        public required int BuildingCount { get; set; }
        public required string AdminName { get; set; }
    }
}
