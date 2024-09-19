using AutoMapper;
using Backend.Common.Profiles;
using Backend.Features.Unit.Commands.Update;
using Backend.Models;
using IntelificioBackTest.Fixtures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace IntelificioBackTest.Features.Unit.Commands
{
    public class UpdateUnitCommandTest
    {
        private readonly Mock<ILogger<UpdateUnitCommandHandler>> _logger;
        private readonly IMapper _mapper;
        private readonly IntelificioDbContext _context;
        private readonly UpdateUnitCommandHandler _handler;

        public UpdateUnitCommandTest()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile<UnitProfile>();
            });

            _context = DbContextFixture.GetDbContext();
            _mapper = new Mapper(mapperConfig);
            _logger = new Mock<ILogger<UpdateUnitCommandHandler>>();
            _handler = new UpdateUnitCommandHandler(_context, _logger.Object, _mapper);
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
            var command = UnitFixture.GetUpdateUnitCommandTest();
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);

            var unit = await _context.Units
                .Include(x => x.Building)
                .Include(x => x.UnitType)
                .FirstOrDefaultAsync(x => x.ID == command.Id);

            Assert.NotNull(unit);
            Assert.Equal(command.Floor, unit.Floor);
            Assert.Equal(command.Surface, unit.Surface);
            Assert.Equal(command.UnitTypeId, unit.UnitType.ID);
            Assert.Equal(command.BuildingId, unit.Building.ID);
        }

        [Fact]
        public async void Failure_Handle_UnitNotFoundUpdate()
        {
            // Arrange
            var command = UnitFixture.GetUpdateUnitCommandTest();
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);
            Assert.Equal("La unidad no fue encontrada", result.Error.Message);
        }

        [Fact]
        public async void Failure_Handle_UnitTypeNotFoundUpdate()
        {
            // Arrange
            var command = UnitFixture.GetUpdateUnitCommandTest();
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);

            Assert.Equal("El tipo de unidad no fue encontrado", result.Error.Message);
        }

        [Fact]
        public async void Failure_Handle_BuildingNotFoundUpdate()
        {
            // Arrange
            var command = UnitFixture.GetUpdateUnitCommandTest();
            await DbContextFixture.SeedData(_context);

            // Act
            var result = await _handler.Handle(command, default);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.True(result.IsFailure);
            Assert.Null(result.Response);
            Assert.Null(result.Errors);

            Assert.Equal("El edificio no fue encontrado", result.Error.Message);
        }
    }
}

