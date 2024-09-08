using Intelificio_Back.Models;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IntelificioBackTest.Mocks
{
    public static class UserManagerMock
    {
        public static Mock<UserManager<User>> CreateUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            var userManager = new Mock<UserManager<User>>(
                store.Object,
                    null, null, null, null, null, null, null, null);
            return userManager;
        }
    }
}
