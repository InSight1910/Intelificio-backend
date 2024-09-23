using Backend.Features.Unit.Queries.GetByID;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Unit.Queries
{
    public class GetByIdUnitTest
    {
        private readonly IntelificioDbContext _context;
        private readonly Mock<ILogger<GetByIDQueryHandler>> _logger;
        private readonly GetByIDQueryHandler _handler;

        public GetByIdUnitTest()
        {
            _context = DbContextFixture.GetDbContext();
            _logger = new Mock<ILogger<GetByIDQueryHandler>>();
            _handler = new GetByIDQueryHandler(_context, _logger.Object);
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            await DbContextFixture.SeedData(_context);

            var unit = _context.Units.First(x => x.ID == 1);

            var query = new GetByIDQuery { UnitId = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);

            var data = result.Response.Data as GetByIDQueryResponse;

            Assert.Equal(unit.Number, data.Number);
            Assert.Equal(unit.Floor, data.Floor);
            Assert.Equal(unit.Surface, data.Surface);
            Assert.Equal(unit.UnitType.Description, data.UnitType);
            Assert.Equal(unit.Building.Name, data.Building);
        }

        [Fact]
        public async Task Failure_Handle_UnitNotFoundGetByID()
        {
            // Arrange
            var query = new GetByIDQuery { UnitId = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailure);
            Assert.False(result.IsSuccess);
            Assert.Equal("La unidad no fue encontrada", result.Error.Message);
        }
    }
}