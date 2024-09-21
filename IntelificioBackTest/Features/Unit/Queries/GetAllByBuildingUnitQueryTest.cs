using Backend.Features.Unit.Queries.GetAllByBuilding;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;


namespace IntelificioBackTest.Features.Unit.Queries
{
    public class GetAllByBuildingTest
    {
        private readonly IntelificioDbContext _context;
        private readonly Mock<ILogger<GetAllByBuildingQueryHandler>> _logger;

        public GetAllByBuildingTest()
        {
            _context = DbContextFixture.GetDbContext();
            _logger = new Mock<ILogger<GetAllByBuildingQueryHandler>>();
        }

        public void Dispose()
        {
            _ = _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            await DbContextFixture.SeedData(_context);
            var unit = _context.Units.FirstOrDefaultAsync(x => x.ID == 1);
            var query = new GetAllByBuildingQuery { BuildingId = 1 };
            var handler = new GetAllByBuildingQueryHandler(_context, _logger.Object);

            // act
            var result = await handler.Handle(query, CancellationToken.None);


            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Response);
            Assert.NotNull(result.Response.Data);

        }

        [Fact]
        public async Task GetAllByBuilding_UnitNotFoundGetAllByBuilding()
        {
            // Arrange
            var query = new GetAllByBuildingQuery { BuildingId = 1 };
            var handler = new GetAllByBuildingQueryHandler(_context, _logger.Object);

            // Act

            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal("La unidad no fue encontrada", result.Error.Message);
        }
    }
}