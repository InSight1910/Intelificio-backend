using Backend.Features.Community.Commands.AddUser;
using Backend.Features.Community.Commands.Assign;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Community.Commands
{
    public class AddUserCommunityCommandTest
    {
        private readonly AddUserCommunityCommandHandler _handler;
        private readonly IntelificioDbContext _context;
        private readonly Mock<ILogger<AddUserCommunityCommandHandler>> _logger;

        public AddUserCommunityCommandTest()
        {
            _logger = new Mock<ILogger<AddUserCommunityCommandHandler>>();
            _context = DbContextFixture.GetDbContext();
            _handler = new AddUserCommunityCommandHandler(_context, _logger.Object);
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
            var command = new AddUserCommunityCommand
            {
                User = new Backend.Features.Community.Commands.AddUser.AddUserObject
                {
                    CommunityId = 1,
                    UserId = 1
                }
            };
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
        }

        [Fact]
        public async void Failure_Handle_Community_Not_Found()
        {
            // Arrange
            var command = new AddUserCommunityCommand
            {
                User = new Backend.Features.Community.Commands.AddUser.AddUserObject
                {
                    CommunityId = 0,
                    UserId = 1
                }
            };
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);

            Assert.Equal("La comunidad indicada no se encuentra registrada en nuestro sistema.", result.Error.Message);
        }

        [Fact]
        public async void Failure_Handle_User_Not_Found()
        {
            // Arrange
            var command = new AddUserCommunityCommand
            {
                User = new Backend.Features.Community.Commands.AddUser.AddUserObject
                {
                    CommunityId = 0,
                    UserId = 1
                }
            };
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("El usuario indicado no se encuentra registrado en nuestro sistema.", result.Error.Message);
        }

        [Fact]
        public async void Failure_Handle_User_Already_Assigned()
        {
            // Arrange
            var command = new AddUserCommunityCommand
            {
                User = new Backend.Features.Community.Commands.AddUser.AddUserObject
                {
                    CommunityId = 0,
                    UserId = 1
                }
            };
            await DbContextFixture.SeedData(_context);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.User.UserId);
            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == command.User.CommunityId);

            community.Users.Add(user);

            // Act

            var result = await _handler.Handle(command, default);


            // Assert
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("El usuario ya se encuentra asignado a la comunidad indicada.", result.Error.Message);
        }
    }
}
