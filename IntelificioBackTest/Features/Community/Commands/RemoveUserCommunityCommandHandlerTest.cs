using Backend.Features.Community.Commands.RemoveUser;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using IntelificioBackTest.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Community.Commands
{
    public class RemoveUserCommunityCommandHandlerTest
    {
        private readonly RemoveUserCommunityCommandHandler _handler;
        private readonly IntelificioDbContext _context;
        private readonly Mock<ILogger<RemoveUserCommunityCommandHandler>> _logger;

        public RemoveUserCommunityCommandHandlerTest()
        {
            _logger = new Mock<ILogger<RemoveUserCommunityCommandHandler>>();
            _context = DbContextFixture.GetDbContext();
            _handler = new RemoveUserCommunityCommandHandler(_context, _logger.Object);
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
            var command = new RemoveUserCommunityCommand
            {
                CommunityId = 1,
                UserId = 1
            };
            await DbContextFixture.SeedData(_context);

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == command.UserId);
            var community = await _context.Community.FirstOrDefaultAsync(x => x.ID == command.CommunityId);

            community.Users.Add(user);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);

            LoggerHelper.AssertLog(_logger, LogLevel.Information, "Removing user from community start");
            LoggerHelper.AssertLog(_logger, LogLevel.Debug, "RemoveUserCommunityCommand: Method: Community.Command.RemoveUserCommunity request: UserID: 1, CommunityId: 1");
            LoggerHelper.AssertLog(_logger, LogLevel.Information, "Removing user from community finish");
        }

    }
}
