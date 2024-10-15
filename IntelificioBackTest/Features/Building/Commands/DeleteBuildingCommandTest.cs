using Backend.Features.Buildings.Commands.Delete;
using Backend.Features.Buildings.Common;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Building.Commands
{
    public class DeleteBuildingCommandTest
    {
        private readonly DeleteBuildingHandler _handler;
        private readonly IntelificioDbContext _context;
        private readonly Mock<ILogger<DeleteBuildingHandler>> _logger;

        public DeleteBuildingCommandTest()
        {
            _logger = new Mock<ILogger<DeleteBuildingHandler>>();
            _context = DbContextFixture.GetDbContext();
            _handler = new DeleteBuildingHandler(_context, _logger.Object);
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
            var command = new DeleteBuildingCommand { Id = 5 };
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
        }

        [Fact]
        public async Task Failure_Handle_Building_Not_Found()
        {
            // Arrange
            var command = new DeleteBuildingCommand { Id = 0 };
            await DbContextFixture.SeedData(_context);

            // Assert
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("Edificio no fue encontrado.", result.Error.Message);
        }

        [Fact]
        public async Task Failure_Handle_Building_Has_Assigned_Units()
        {
            // Arrange
            await DbContextFixture.SeedData(_context);

            var command = new DeleteBuildingCommand { Id = 1 };

            // Assert
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("El Edificio tiene unidades asignadas.", result.Error.Message);
        }
    }

}
