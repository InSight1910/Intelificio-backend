using Backend.Features.Community.Queries.GetAllByUser;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using IntelificioBackTest.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Community.Queries
{
    public class GetAllByUserCommunityQueryTest
    {
        private readonly IntelificioDbContext _context;
        private readonly Mock<ILogger<GetAllByUserQueryHandler>> _logger;
        private readonly GetAllByUserQueryHandler _handler;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<RoleManager<Role>> _roleManager;

        public GetAllByUserCommunityQueryTest()
        {
            _context = DbContextFixture.GetDbContext();
            _logger = new Mock<ILogger<GetAllByUserQueryHandler>>();
            _userManager = UserManagerMock.CreateUserManager();
            _roleManager = new Mock<RoleManager<Role>>();
            _handler = new GetAllByUserQueryHandler(_userManager.Object, _roleManager.Object, _context, _logger.Object);
        }

        public void Dispose()
        {
            _ = _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllByUserQueryHandler_UserNotFound_ReturnsFailure()
        {
            // Arrange
            var query = new GetAllByUserQuery { UserId = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("El usuario no se encuentra en nuestros registros.", result.Error.Message);
        }


        [Fact]
        public async Task GetAllByUserQueryHandler_UserFound_Success()
        {
            // Arrange
            await DbContextFixture.SeedData(_context);
            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == 1);
            community!.Users.Add(await _context.Users.FirstAsync(x => x.Id == 1));
            _ = await _context.SaveChangesAsync();

            var query = new GetAllByUserQuery { UserId = 1 };


            // Act
            var result = await _handler.Handle(query, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Response);

            var data = result.Response.Data as ICollection<GetAllByUserResponse>;

            Assert.NotEmpty(data!);
            Assert.Equal(1, data!.Count);
            Assert.Equal("Admin Admin", data.First().AdminName);
        }

        [Fact]
        public async Task GetAllByUserQueryHandler_UserFound_Success_WithOutAdmin()
        {
            // Arrange
            await DbContextFixture.SeedData(_context);
            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == 1);
            community!.Users.Add(await _context.Users.FirstAsync(x => x.Id == 2));
            _ = await _context.SaveChangesAsync();


            var query = new GetAllByUserQuery { UserId = 2 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Response);

            var response = result.Response;
            Assert.NotNull(response.Data as ICollection<GetAllByUserResponse>);

            var data = response.Data as ICollection<GetAllByUserResponse>;

            Assert.NotEmpty(data!);
            Assert.Equal(1, data!.Count);
            Assert.Equal("Sin Administrador", data.First().AdminName);
        }
    }
}
