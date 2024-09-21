namespace Backend.Features.Authentication.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryResponse
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Role { get; set; }
    }
}
