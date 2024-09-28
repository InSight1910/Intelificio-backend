using Backend.Features.Community.Queries.GetById;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using IntelificioBackTest.Mocks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Community.Queries
{
    public class GetByIdCommunityQueryTest
    {
        private readonly IntelificioDbContext _context;
        private readonly Mock<ILogger<GetByIdCommunityQueryHandler>> _logger;
        private readonly GetByIdCommunityQueryHandler _handler;
        private readonly Mock<UserManager<User>> _userManager;
        private readonly Mock<RoleManager<Role>> _roleManager;

        public GetByIdCommunityQueryTest()
        {
            _context = DbContextFixture.GetDbContext();
            _logger = new Mock<ILogger<GetByIdCommunityQueryHandler>>();
            _userManager = UserManagerMock.CreateUserManager();
            _roleManager = new Mock<RoleManager<Role>>();
            _handler = new GetByIdCommunityQueryHandler(_context, _userManager.Object, _logger.Object);
        }

        [Fact]
        public async Task GetByIdCommunityQueryHandler_WhenCommunityDoesNotExist_ReturnsError()
        {
            // Arrange
            var query = new GetByIdCommunityQuery { Id = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Equal("La comunidad indicada no se encuentra dentro de nuestros registros.", result.Error.Message);
        }

        [Fact]
        public async Task GetByIdCommunityQueryHandler_WhenCommunityExists_ReturnsCommunity()
        {
            // Arrange
            await DbContextFixture.SeedData(_context);

            var community = _context.Community.First(x => x.ID == 1);

            var query = new GetByIdCommunityQuery { Id = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var data = result.Response.Data as GetByIdCommunityResponse;

            Assert.Equal(community.Name, data.Name);
            Assert.Equal(community.Address, data.Address);
            Assert.Equal(community.Municipality.ID, data.MunicipalityId);
            Assert.Equal(community.Municipality.City.ID, data.CityId);
            Assert.Equal(community.Municipality.City.Region.ID, data.RegionId);
        }
    }
}
