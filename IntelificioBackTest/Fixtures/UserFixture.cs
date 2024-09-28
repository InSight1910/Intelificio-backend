using Backend.Models;

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
                PhoneNumber = "123",
                Rut = "123",
                UserName = "test@test.com",
                EmailConfirmed = true,
            };
        }
    }
}
