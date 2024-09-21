namespace Backend.Features.Community.Queries.GetUsersByCommunity
{
    public class GetUsersByCommunityQueryResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required int UnitCount { get; set; }
        public required string Role { get; set; }
    }
}
