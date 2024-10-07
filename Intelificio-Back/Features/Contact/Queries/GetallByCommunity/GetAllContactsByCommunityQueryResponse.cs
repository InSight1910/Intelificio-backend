namespace Backend.Features.Contact.Queries.GetallByCommunity
{
    public class GetAllContactsByCommunityQueryResponse
    {
        public required int Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Service { get; set; }
        public int CommunityId { get; set; }
    }
}
