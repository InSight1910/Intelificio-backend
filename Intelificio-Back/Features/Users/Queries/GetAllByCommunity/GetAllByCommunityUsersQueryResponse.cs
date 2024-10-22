namespace Backend.Features.Users.Queries.GetAllByCommunity
{
    public class GetAllByCommunityUsersQueryResponse
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Rut { get; set; }
        public required int CommunityId { get; set; }
    }
}
