using Backend.Features.Unit.Commands.Delete;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Unit.Commands
{
    public class DeleteUnitCommandTest
    {
        private readonly DeleteUnitCommandHandler _handler;
        private readonly Mock<ILogger<DeleteUnitCommandHandler>> _logger;
        private readonly IntelificioDbContext _context;

        public DeleteUnitCommandTest()
        {
            _logger = new Mock<ILogger<DeleteUnitCommandHandler>>();
            _context = DbContextFixture.GetDbContext();
            _handler = new DeleteUnitCommandHandler(_context, _logger.Object);
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
            var command = new DeleteUnitCommand { Id = 1 };

            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
        }

        [Fact]
        public async Task Failure_Handle_UnitNotFoundDelete()
        {
            // Arrange
            var command = new DeleteUnitCommand { Id = 1 };

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("La unidad no fue encontrada", result.Error.Message);
        }
    }
}
