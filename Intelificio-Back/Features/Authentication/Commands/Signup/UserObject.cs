namespace Backend.Features.Authentication.Commands.Signup
{
    public class UserObject
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required string Rut { get; set; }
        public required string Role { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
