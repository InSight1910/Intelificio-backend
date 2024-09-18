using AutoMapper;
using Backend.Common.Profiles;
using Backend.Features.Building.Commands.Update;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;


namespace IntelificioBackTest.Features.Building.Commands
{
    public class UpdateBuildingCommandTest
    {
        private readonly UpdateBuildigCommandHandler _handler;
        private readonly IntelificioDbContext _context;
        private readonly Mock<ILogger<UpdateBuildigCommandHandler>> _logger;
        private readonly IMapper _mapper;

        public UpdateBuildingCommandTest()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<BuildingProfile>();
            });

            _mapper = new Mapper(mapperConfig);
            _logger = new Mock<ILogger<UpdateBuildigCommandHandler>>();
            _context = DbContextFixture.GetDbContext();
            _handler = new UpdateBuildigCommandHandler(_context, _logger.Object, _mapper);
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
            var command = new UpdateBuildingCommand
            {
                Id = 1,
                Name = "Juana",
                Floors = 10,
                CommunityId = 1
            };
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.False(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
        }

    }
}
