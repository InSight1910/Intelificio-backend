using Backend.Features.Unit.Commands.RemoveUser;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Unit.Commands
{
    public class RemoveUserUnitCommandTest
    {
        private readonly RemoveUserUnitCommandHandler _handler;
        private readonly IntelificioDbContext _context;
        private readonly Mock<ILogger<RemoveUserUnitCommandHandler>> _logger;

        public RemoveUserUnitCommandTest()
        {
            _logger = new Mock<ILogger<RemoveUserUnitCommandHandler>>();
            _context = DbContextFixture.GetDbContext();
            _handler = new RemoveUserUnitCommandHandler(_context, _logger.Object);
        }

        public void Dispose()
        {
            _ = _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async void Handle_Success()
        {
            // Arrange
            var command = new RemoveUserUnitCommand
            {
                UnitId = 1,
                UserId = 1
            };
            await DbContextFixture.SeedData(_context);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.UserId);
            var unit = await _context.Units.FirstOrDefaultAsync(x => x.ID == command.UnitId);

            unit.Users.Add(user);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
        }

        [Fact]
        public async void Handle_Failure_UserNotFound()
        {
            // Arrange
            var command = new RemoveUserUnitCommand
            {
                UnitId = 1,
                UserId = 0
            };
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("El usuario no fue encontrado", result.Error.Message);
        }

        [Fact]
        public async void Handle_Failure_UnitNotFound()
        {
            var command = new RemoveUserUnitCommand
            {
                UnitId = 0,
                UserId = 1
            };
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("La unidad no fue encontrada", result.Error.Message);
        }

        [Fact]
        public async void Handle_Failure_UserIsNotAssigned()
        {
            var command = new RemoveUserUnitCommand
            {
                UnitId = 1,
                UserId = 1
            };
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("El usuario no ha sido registrado", result.Error.Message);
        }
    }
}

