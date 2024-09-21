using Backend.Features.Buildings.Queries.GetAllByCommunity;
using Backend.Models;
using Castle.Core.Logging;
using IntelificioBackTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelificioBackTest.Features.Building.Queries
{
    public class GetAllByCommunityQueryTest
    {
        private readonly IntelificioDbContext _context;
        private readonly Mock<ILogger<GetAllByCommunityQueryHandler>> _logger;

        public GetAllByCommunityQueryTest()
        {
            _context = DbContextFixture.GetDbContext();
            _logger = new Mock<ILogger<GetAllByCommunityQueryHandler>>();
        }

        public void Dispose()
        {
            _ = _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetAllByCommunity_CommunityNotFound() 
        {
            // Arrange
            var query = new GetAllByCommunityQuery { CommunityId = 0 };
            var handler = new GetAllByCommunityQueryHandler(_context, _logger.Object);

            // Act

            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal("Comunidad no fue encontrada.", result.Error.Message);
        }

        [Fact]
        public async Task GetAllByCommunity_Success()
        {
            // Arrange
            await DbContextFixture.SeedData(_context);
            var community = _context.Community.First(x => x.ID == 2);
            var query = new GetAllByCommunityQuery { CommunityId = 2 };
            var handler = new GetAllByCommunityQueryHandler(_context, _logger.Object);

            // act
            var result = await handler.Handle(query, CancellationToken.None);


            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Response);
            Assert.NotNull(result.Response.Data);

        }

    }
}
