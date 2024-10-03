namespace Backend.Features.Authentication.Queries.GetUserByEmail
{
    public class GetUserByEmailQueryResponse
    {
        public required int Id { get; set; }
        public required string FullName { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Role { get; set; }
    }
}
