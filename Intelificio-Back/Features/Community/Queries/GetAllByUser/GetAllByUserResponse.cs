namespace Backend.Features.Community.Queries.GetAllByUser
{
    public class GetAllByUserResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public int UnitCount { get; set; }
        public int BuildingCount { get; set; }
        public required string AdminName { get; set; }
    }
}
