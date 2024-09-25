namespace Backend.Features.Authentication.Queries.GetAllUserAdmin
{
    public class GetAllUserAdminQueryResponse
    {
        public required int id { get; set; }
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Rut { get; set; }
    }
}
