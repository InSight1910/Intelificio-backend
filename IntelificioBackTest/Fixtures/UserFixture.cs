using Intelificio_Back.Models;

namespace IntelificioBackTest.Fixtures
{
    internal static class UserFixture
    {
        public static User GetUserTest()
        {
            return new()
            {
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "LastTest",
                Password = "Test",
                PhoneNumber = "123",
                Rut = "123",
                Role = new Role
                {
                    Name = "Role"
                },
                UserName = "test@test.com",
                EmailConfirmed = true,
            };
        }
    }
}
