using Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace IntelificioBackTest.Mocks;

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

    public static Mock<RoleManager<Role>> CreateRoleManager()
    {
        var store = new Mock<IRoleStore<Role>>();
        var roleManager = new Mock<RoleManager<Role>>(
            store.Object,
            null,
            null,
            null,
            null);
        return roleManager;
    }

    public static Mock<SignInManager<User>> CreateSignInManager(UserManager<User> userManager)
    {
        var store = new Mock<IUserStore<User>>();
        var signInManagerMock = new Mock<SignInManager<User>>(
            userManager,
            Mock.Of<IHttpContextAccessor>(),
            Mock.Of<IUserClaimsPrincipalFactory<User>>(),
            null,
            null,
            null,
            null);
        return signInManagerMock;
    }
}