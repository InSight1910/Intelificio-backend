using Backend.Features.Building.Commands.Create;
using Backend.Models;
using Microsoft.Extensions.Logging;
using Moq;
using AutoMapper;
using Backend.Common.Profiles;
using IntelificioBackTest.Fixtures;
using Backend.Features.Building.Common;
using Backend.Features.Buildings.Commands.Create;

namespace IntelificioBackTest.Features.Building.Commands
{
    public class CreateBuildingCommandTest
    {
        private readonly CreateBuildingCommandHandler _handler;
        private readonly IntelificioDbContext _context;
        private readonly Mock<ILogger<CreateBuildingCommandHandler>> _logger;
        private IMapper _mapper;

        public CreateBuildingCommandTest()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<BuildingProfile>();
            });

            _mapper = new Mapper(mapperConfig);
            _logger = new Mock<ILogger<CreateBuildingCommandHandler>>();
            _context = DbContextFixture.GetDbContext();
            _handler = new CreateBuildingCommandHandler(_context, _logger.Object, _mapper);
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
            var command = BuildingFixture.GetCreateBuildingCommandTest();
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
        public async Task Failure_Handle_Building_Without_Floors()
        {
            // Arrange
            var command = new CreateBuildingCommand
            {
                Name = "A",
                Floors = 0,
                CommunityId = 1
            };
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("El edificio debe tener al menos 1 piso asignado.", result.Error.Message);
        }

        [Fact]
        public async Task Failure_Handle_Community_Not_Found()
        {
            // Arrange
            var command = BuildingFixture.GetCreateBuildingCommandTest();
            await DbContextFixture.SeedData(_context);

            command.CommunityId = 0;

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("Comunidad no fue encontrada.", result.Error.Message);
        }
    }
}
