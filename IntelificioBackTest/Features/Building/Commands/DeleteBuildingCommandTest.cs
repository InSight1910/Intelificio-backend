using Backend.Features.Building.Commands.Delete;
using Backend.Features.Building.Common;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelificioBackTest.Features.Building.Commands
{
    public class DeleteBuildingCommandTest
    {
        private readonly DeleteBuildingCommandHandler _handler;
        private readonly IntelificioDbContext _context;
        private readonly Mock<ILogger<DeleteBuildingCommandHandler>> _logger;

        public DeleteBuildingCommandTest()
        {
            _logger = new Mock<ILogger<DeleteBuildingCommandHandler>>();
            _context = DbContextFixture.GetDbContext();
            _handler = new DeleteBuildingCommandHandler(_context, _logger.Object);
        }

        [Fact]
        public void Dispose()
        {
            _ = _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task Handle_Success()
        {
            // Arrange
            var command = new DeleteBuildingCommand { Id = 1 };
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
            Assert.Contains(result.Error.Message, BuildingErrors.BuildingNotFoundOnDelete.Message);
        }

        [Fact]
        public async Task Failure_Handle_Building_Has_Assigned_Units()
        {
            // Arrange
            var db = DbContextFixture.GetDbContext();
            await DbContextFixture.SeedData(_context);

            var command = new DeleteBuildingCommand { Id = 1 };

            // Assert
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Contains(result.Error.Message, BuildingErrors.HasAssignedUnitsOnDelete.Message);
        }
    }

}
